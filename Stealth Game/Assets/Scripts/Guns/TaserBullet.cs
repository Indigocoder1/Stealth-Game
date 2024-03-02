using UnityEngine;

public class TaserBullet : MultiplayerBullet
{
    public float expireTime;

    protected override void OnBulletImpact(Collision collision)
    {
        owner.RemoveActiveBullet(this);
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            DamageTarget(damageable);
        }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.parent = collision.transform;

        Destroy(gameObject, expireTime);
    }

    public void SetExpireTime(float expireTime)
    {
        this.expireTime = expireTime;
    }
}
