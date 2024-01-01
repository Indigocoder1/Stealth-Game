using UnityEngine;
using Photon.Pun;

public class TaserBullet : MonoBehaviour
{
    private int team = 0;
    private int damage = 0;

    [SerializeField] private PhotonView photonView;


    public int GetTeam()
    {
        return team;
    }

    public int GetDamage()
    {
        Debug.Log("Damage Got: " + damage);
        return damage;
    }

    public void SetTeam(int teamInput)
    {
        //SetTeamForAll(teamInput);
        photonView.RPC("SetTeamForAll", RpcTarget.All, teamInput);
    }
    public void SetDamage(int damageInput)
    {
        //SetDamageForAll(damageInput);
        photonView.RPC("SetDamageForAll", RpcTarget.All, damageInput);
    }

    [PunRPC]
    public void SetTeamForAll(int teamInput)
    {
        team = teamInput;
    }

    [PunRPC]
    public void SetDamageForAll(int damageInput)
    {
        damage = damageInput;
        Debug.Log("Damage Set: " + damage);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collided");
    //    IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
    //    if (damageable != null)
    //    {
    //        DamageTarget(damageable);
    //    }

    //    Destroy(gameObject);
    //}

    //protected virtual void DamageTarget(IDamageable damageable)
    //{
    //    Debug.Log("trying to damage");
    //    damageable.Damage(damage);
    //}
}
