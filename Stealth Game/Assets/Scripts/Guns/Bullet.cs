using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int team;
    private int damage;

    public void SetTeam(int teamInput)
    {
        team = teamInput;
    }

    public int GetTeam() 
    {
        return team;
    }

    public void SetDamage(int damageInput)
    {
        damage = damageInput;
    }

    public int GetDamage()
    {
        return damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        //if (damageable != null)
        //{
        //    DamageTarget(damageable);
        //}

        Destroy(gameObject);
    }

    //protected virtual void DamageTarget(IDamageable damageable)
    //{
       //damageable.Damage(damage);
    //}
}
