using System;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenShots;
    public float noGravityRepulseForce;
    public float gravityRepulseForce;
    [Tooltip("Delay between switching weapons (Animation delay)")]
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

    [Header("Kickback")]
    public KickbackManager kickbackManager;
    public float kickback;
    [Range(-1, 1)]
    public float horizontalKickback;
    [Range(-1, 1)]
    public float verticalKickback;
    public float snappinessOverride;
    public float returnSpeedOverride;

    private float timeSinceLastShot;
    private PlayerInputActions inputActions;
    protected bool isConnected = PhotonNetwork.IsConnected;
    protected List<Bullet> activeBullets = new List<Bullet>();

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        if(fireType == FireType.SemiAuto)
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

        recoilManager.AddRecoil(-verticalRecoil, horizontalRecoil + Random.Range(-horizontalRandomness, horizontalRandomness));

        float horizontalKickback = Random.Range(-this.horizontalKickback, this.horizontalKickback);
        float verticalKickback = Random.Range(-this.verticalKickback, this.verticalKickback);
        kickbackManager.AddKickback(new Vector3(horizontalKickback, verticalKickback, kickback), snappinessOverride, returnSpeedOverride);

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

        if(fireType == FireType.Auto && inputActions.Player.Fire.IsPressed())
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
    Auto,
    SemiAuto
}