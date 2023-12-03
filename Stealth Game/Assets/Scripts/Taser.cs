using UnityEditor.Media;
using UnityEngine;

public class Taser : Gun
{
    [Header("Other")]
    public LineFX lineFX;

    private Bullet shotBullet;

    private void Start()
    {
        lineFX.SetOrigin(bulletSpawnPosition);
    }

    protected override void HandleProjectileFire()
    {
        base.HandleProjectileFire();

        shotBullet = activeBullets[activeBullets.Count - 1];

        lineFX.SetTarget(shotBullet.transform);
    }

    protected override void Update()
    {
        base.Update();
        lineFX.DrawRope();
    }
}
