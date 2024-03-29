using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ProjectileGun : Gun
{
    [Header("Projectile")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    public float bulletSpeed;
    public float maxBulletLifetime;

    protected override void HandleFire()
    {
        GameObject bulletGameobject;
        if (isConnected)
        {
            bulletGameobject = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        }
        else
        {
            bulletGameobject = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        }

        bulletGameobject.transform.forward = -cameraTransform.forward;
        Rigidbody bulletRB = bulletGameobject.GetComponent<Rigidbody>();
        bulletRB.AddForce(cameraTransform.forward * bulletSpeed);

        Bullet bullet = bulletGameobject.GetComponent<Bullet>();
        bullet.SetDamage(shotDamage);
        bullet.SetOwner(this);
        bullet.SetLifetime(maxBulletLifetime);

        activeBullets.Add(bullet);

        if (isConnected) bullet.SetTeam(player.GetComponent<TeamMember>().GetTeam());
        if (isConnected) StartCoroutine(PhotonDestroyDelayed(bulletGameobject, maxBulletLifetime));
        else Destroy(bulletGameobject, maxBulletLifetime);
    }

    private IEnumerator PhotonDestroyDelayed(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject);
    }
}
