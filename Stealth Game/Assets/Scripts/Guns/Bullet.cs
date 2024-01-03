using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    protected int team = 0;
    protected int damage = 0;
    protected PhotonView photonView;
    protected bool inMultiplayer = PhotonNetwork.IsConnected;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public int GetTeam()
    {
        return team;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetTeam(int team)
    {
        if(inMultiplayer) photonView.RPC("SetTeamForAll", RpcTarget.All, team);
        else this.team = team;
    }

    public void SetDamage(int damage)
    {
        if(inMultiplayer) photonView.RPC("SetDamageForAll", RpcTarget.All, damage);
        else this.damage = damage;
    }

    [PunRPC]
    public void SetTeamForAll(int team)
    {
        this.team = team;
    }

    [PunRPC]
    public void SetDamageForAll(int damage)
    {
        this.damage = damage;
        Debug.Log("Damage Set: " + this.damage);
    }

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