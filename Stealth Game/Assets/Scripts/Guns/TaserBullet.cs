using UnityEngine;
using Photon.Pun;

public class TaserBullet : Bullet
{
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            DamageTarget(damageable);
        }

        Destroy(gameObject);
    }

    protected virtual void DamageTarget(IDamageable damageable)
    {
        Debug.Log("trying to damage");
        damageable.Damage(damage);
    }
}
