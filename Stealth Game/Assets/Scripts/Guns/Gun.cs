using System;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenShots;
    public float noGravityRepulseForce;
    public float gravityRepulseForce;
    public float holsterTime;
    public int shotDamage;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;
    public Player player;
    public FireType fireType;

    [Header("Recoil")]
    public RecoilManager recoilManager;
    public float verticalRecoil;
    public float horizontalRecoil;
    [Range(0, 5f)]
    public float horizontalRandomness;

    private float timeSinceLastShot;
    private PlayerInputActions inputActions;
    protected bool isConnected = PhotonNetwork.IsConnected;
    protected List<Bullet> activeBullets = new List<Bullet>();

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        if(fireType == FireType.Click)
        {
            inputActions.Player.Fire.performed += _ => TryFire();
        }
    }

    private void Start()
    {
        timeSinceLastShot = timeBetweenShots;
    }

    protected virtual void TryFire()
    {
        if (timeSinceLastShot < timeBetweenShots)
        {
            return;
        }

        recoilManager.AddRecoil(-verticalRecoil, horizontalRecoil + UnityEngine.Random.Range(-horizontalRandomness, horizontalRandomness));

        bool inGravity = player.InGravity();
        float repulseForce = inGravity ? gravityRepulseForce : noGravityRepulseForce;
        playerRigidbody.AddForce(-cameraTransform.forward * repulseForce * 10f, ForceMode.Force);
        timeSinceLastShot = 0;

        HandleFire();
    }

    protected virtual void HandleFire()
    {
        throw new NotImplementedException();
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

        if(fireType == FireType.Hold && inputActions.Player.Fire.IsPressed())
        {
            TryFire();
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
    Hold,
    Click
}