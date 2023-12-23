using UnityEngine;

public class TaserRound : Bullet
{
    public float destroyTime;

    protected override void OnBulletImpact(Collision collision)
    {
        base.OnBulletImpact(collision);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.parent = collision.transform;

        Destroy(gameObject, destroyTime);
    }
}
