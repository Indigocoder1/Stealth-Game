using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenShots;
    public float noGravityRepulseForce;
    public float gravityRepulseForce;
    public int shotDamage;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;
    public Player player;
    public FireType fireType;

    [Header("Projectile")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    public float bulletSpeed;
    public float maxBulletLifetime;

    [Header("Hitscan")]
    public float maxDistance;
    public LayerMask allowedHitMask;

    [Header("Recoil")]
    public RecoilManager recoilManager;
    public float verticalRecoil;
    public float horizontalRecoil;
    [Range(0, 5f)]
    public float horizontalRandomness;

    private float timeSinceLastShot;
    private PlayerInputActions inputActions;
    protected event EventHandler onHitscanHit;
    protected bool isConnected = PhotonNetwork.IsConnected;
    protected List<Bullet> activeBullets = new List<Bullet>();

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Fire.performed += _ => Fire();
    }

    private void Start()
    {
        timeSinceLastShot = timeBetweenShots;
    }

    protected virtual void Fire()
    {
        if (timeSinceLastShot < timeBetweenShots)
        {
            return;
        }

        switch (fireType)
        {
            case FireType.Projectile:
                HandleProjectileFire();
                break;
            case FireType.Hitscan:
                HandleHitscanFire();
                break;
        }

        recoilManager.AddRecoil(-verticalRecoil, horizontalRecoil + UnityEngine.Random.Range(-horizontalRandomness, horizontalRandomness));

        bool inGravity = player.InGravity();
        float repulseForce = inGravity ? gravityRepulseForce : noGravityRepulseForce;
        playerRigidbody.AddForce(-cameraTransform.forward * repulseForce * 10f, ForceMode.Force);
        timeSinceLastShot = 0;
    }

    protected virtual void HandleProjectileFire()
    {
        GameObject bulletGameobject;
        if (isConnected)
        {
            bulletGameobject = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        }
        else
        {
            bulletGameobject = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        }

        bulletGameobject.transform.forward = -cameraTransform.forward;
        Rigidbody bulletRB = bulletGameobject.GetComponent<Rigidbody>();
        bulletRB.AddForce(cameraTransform.forward * bulletSpeed);

        Bullet bullet = bulletGameobject.GetComponent<Bullet>();
        bullet.SetDamage(shotDamage);
        bullet.SetOwner(this);
        bullet.SetLifetime(maxBulletLifetime);

        activeBullets.Add(bullet);

        if (isConnected) bullet.SetTeam(player.GetComponent<TeamMember>().GetTeam());
        if (isConnected) StartCoroutine(PhotonDestroyDelayed(bulletGameobject, maxBulletLifetime));
        else Destroy(bulletGameobject, maxBulletLifetime);
    }

    private IEnumerator PhotonDestroyDelayed(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject);
    }

    protected virtual void HandleHitscanFire()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, maxDistance, allowedHitMask))
        {
            onHitscanHit?.Invoke(this, EventArgs.Empty);

            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(shotDamage);
            }
        }
    }

    public void RemoveActiveBullet(Bullet bullet)
    {
        activeBullets.Remove(bullet);
    }

    protected virtual void Update()
    {
        if (timeSinceLastShot < timeBetweenShots)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}

public enum FireType
{
    Projectile,
    Hitscan
}