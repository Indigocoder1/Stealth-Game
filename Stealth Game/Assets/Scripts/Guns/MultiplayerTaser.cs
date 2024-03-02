using System;
using UnityEngine;
using Photon.Pun;

public class MultiplayerTaser : ProjectileGun
{
    [Header("Effects")]
    public LineFX lineFX;
    public GameObject ropeLightningPrefab;
    public GameObject impactLightningPrefab;

    private MultiplayerBullet shotBullet;
    private ParticleSystem ropeLightning;

    private float taserSpeed = 24f;

    private void Start()
    {
        lineFX.SetOrigin(bulletSpawnPosition);
        ropeLightningPrefab.SetActive(false);
        ropeLightning = PhotonNetwork.Instantiate(ropeLightningPrefab.name, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation).GetComponent<ParticleSystem>();
    }

    protected override void HandleFire()
    {
        Debug.Log("Mutliplayer Taser Handling Fire");
        base.HandleFire();

        shotBullet = activeBullets[activeBullets.Count - 1];

        lineFX.SetTarget(shotBullet.transform);
        shotBullet.OnImpact += SpawnLightning;
        
        ropeLightning.Simulate(taserSpeed, true);
        ropeLightning.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        lineFX.DrawRope();

        if (shotBullet)
        {
            ropeLightning.transform.LookAt(shotBullet.transform.position);
            ropeLightning.transform.position = bulletSpawnPosition.position;
        }
        else
        {
            ropeLightning.gameObject.SetActive(false);
        }
    }

    private void SpawnLightning(object sender, EventArgs e)
    {
        Transform senderTransform = ((MultiplayerBullet)sender).transform;
        PhotonNetwork.Instantiate(impactLightningPrefab.name, senderTransform.position, senderTransform.rotation);
    }
}
