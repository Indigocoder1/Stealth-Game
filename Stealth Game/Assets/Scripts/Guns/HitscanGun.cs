using System;
using UnityEngine;

public class HitscanGun : Gun
{
    [Header("Hitscan")]
    public float maxDistance;
    public LayerMask allowedHitMask;
    public GameObject impactEffect;

    public event EventHandler onHitscanHit;

    protected override void HandleFire()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, maxDistance, allowedHitMask))
        {
            onHitscanHit?.Invoke(this, EventArgs.Empty);

            GameObject effect = Instantiate(impactEffect, hit.point + (hit.normal * 0.001f), Quaternion.LookRotation(hit.normal));

            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(shotDamage);
            }
        }
    }
}
