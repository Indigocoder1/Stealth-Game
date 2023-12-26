using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform owner;
    private int damage;

    public void SetOwner(Transform owner)
    {
        this.owner = owner;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            DamageTarget(damageable);
        }

        Destroy(gameObject);
    }

    protected virtual void DamageTarget(IDamageable damageable)
    {
        damageable.Damage(damage);
    }
}
