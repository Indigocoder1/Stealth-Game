using UnityEngine;

public class Taser : Gun
{
    private void Start()
    {
        maxDistance = 200;
        bulletDamage = 30;
    }

    protected override void HandleProjectileFire()
    {
        base.HandleProjectileFire();
        //Other stuff if needed
    }
}
