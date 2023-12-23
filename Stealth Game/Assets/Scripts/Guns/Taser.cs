using System;
using UnityEngine;

public class Taser : Gun
{
    [Header("Effects")]
    public LineFX lineFX;
    public GameObject ropeLightningPrefab;
    public GameObject impactLightningPrefab;

    private Bullet shotBullet;
    private ParticleSystem ropeLightning;

    private void Start()
    {
        lineFX.SetOrigin(bulletSpawnPosition);

        ropeLightning = Instantiate(ropeLightningPrefab, bulletSpawnPosition).GetComponent<ParticleSystem>();
        ropeLightning.gameObject.SetActive(false);
    }

    protected override void HandleProjectileFire()
    {
        base.HandleProjectileFire();

        shotBullet = activeBullets[activeBullets.Count - 1];

        lineFX.SetTarget(shotBullet.transform);
        shotBullet.OnImpact += SpawnLightning;

        ropeLightning.Simulate(0f, true);
        ropeLightning.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        lineFX.DrawRope();

        if(shotBullet)
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
        Transform senderTransform = ((Bullet)sender).transform;
        Instantiate(impactLightningPrefab, senderTransform.position, senderTransform.rotation);
    }
}
