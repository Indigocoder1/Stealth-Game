using UnityEngine;
using Photon.Pun;
using System;

public class Bullet : MonoBehaviour
{
    public event EventHandler OnImpact;
    protected int team;
    protected int damage;
    protected float lifetime;
    protected PhotonView photonView;
    protected bool inMultiplayer = PhotonNetwork.IsConnected;
    protected Gun owner;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SetOwner(Gun owner)
    {
        this.owner = owner;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
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
        Debug.Log($"Damage set to {this.damage}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnImpact?.Invoke(this, EventArgs.Empty);
        OnBulletImpact(collision);
    }

    protected virtual void DamageTarget(IDamageable damageable)
    {
        Debug.Log("Trying to damage");
        damageable.Damage(damage);
    }

    protected virtual void OnBulletImpact(Collision collision)
    {
        owner.RemoveActiveBullet(this);
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            DamageTarget(damageable);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        owner.RemoveActiveBullet(this);
    }
}