using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenShots;
    public float repulseForce;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;
    protected FireType fireType;

    [Header("Projectile")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    public float bulletSpeed;
    public float maxBulletLifetime;

    [Header("Hitscan")]
    public float maxDistance;
    public LayerMask allowedHitMask;

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

        switch(fireType)
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
        bulletRB.velocity = playerRigidbody.velocity;
        bulletRB.AddForce(cameraTransform.forward * bulletSpeed);

        Destroy(bulletGameobject, maxBulletLifetime);
    }

    protected virtual void HandleHitscanFire()
    {
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, maxDistance, allowedHitMask))
        {
            HandleRaycastHit();
            onHitscanHit?.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void HandleRaycastHit()
    {
        //On hit stuff
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
