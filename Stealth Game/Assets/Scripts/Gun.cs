using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenShots;
    public float repulseForce;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;
    public FireType fireType;
    [SerializeField]
    protected int bulletDamage;

    [Header("Projectile")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    public float bulletSpeed;
    public float maxBulletLifetime;

    [Header("Hitscan")]
    public LayerMask allowedHitMask;
    [SerializeField]
    protected float maxDistance;

    private float timeSinceLastShot;
    private PlayerInputActions inputActions;
    protected event EventHandler onHitscanHit;

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

        //TODO - Recoil
        playerRigidbody.AddForce(-cameraTransform.forward * repulseForce * 10f, ForceMode.Force);
        timeSinceLastShot = 0;
    }

    protected virtual void HandleProjectileFire()
    {
        GameObject bulletGameobject = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        bulletGameobject.transform.forward = -cameraTransform.forward;
        Rigidbody bulletRB = bulletGameobject.GetComponent<Rigidbody>();
        bulletRB.AddForce(cameraTransform.forward * bulletSpeed);

        Destroy(bulletGameobject, maxBulletLifetime);
    }

    protected virtual void HandleHitscanFire()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, maxDistance, allowedHitMask))
        {
            HandleRaycastHit(hit);
            onHitscanHit?.Invoke(this, EventArgs.Empty);
        }

        /*Old stuff:
        //graphics
        GameObject bulletGameobject = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        bulletGameobject.transform.forward = -cameraTransform.forward;

        // check if hit anything
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit1, maxDistance, ~0))
        {
            bulletGameobject.transform.localScale = new Vector3(1, 1, hit1.distance);
        }
        else
        {
            bulletGameobject.transform.localScale = new Vector3(1, 1, 10);
        }

        Destroy(bulletGameobject, 0.7f);
        */
    }

    protected virtual void HandleRaycastHit(RaycastHit hitInfo)
    {
        //On hit stuff
        IDamageable damageable = hitInfo.collider.GetComponentInParent<IDamageable>();
        if(damageable != null)
        {
            damageable.Damage(bulletDamage);
        }
    }

    private void Update()
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