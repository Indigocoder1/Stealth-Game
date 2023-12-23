using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenShots;
    public float noGravityRepulseForce;
    public float gravityRepulseForce;
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
        GameObject bulletGameobject = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        bulletGameobject.transform.forward = -cameraTransform.forward;
        Rigidbody bulletRB = bulletGameobject.GetComponent<Rigidbody>();

        Bullet bullet = bulletGameobject.GetComponent<Bullet>();
        bullet.SetOwner(this);
        activeBullets.Add(bullet);

        //bulletRB.velocity = playerRigidbody.velocity;
        //Inherits player velocity ^

        bulletRB.AddForce(cameraTransform.forward * bulletSpeed);

        Destroy(bulletGameobject, maxBulletLifetime);
    }

    protected virtual void HandleHitscanFire()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, maxDistance, allowedHitMask))
        {
            HandleRaycastHit();
            onHitscanHit?.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void HandleRaycastHit()
    {
        //On hit stuff
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