using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public event EventHandler OnImpact;
    protected Gun owner;

    public void SetOwner(Gun owner)
    {
        this.owner = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnImpact?.Invoke(this, EventArgs.Empty);
        OnBulletImpact(collision);
    }

    protected virtual void OnBulletImpact(Collision collision)
    {
        owner.RemoveActiveBullet(this);
    }
}
