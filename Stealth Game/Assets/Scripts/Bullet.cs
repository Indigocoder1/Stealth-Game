using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Gun owner;

    public void SetOwner(Gun owner)
    {
        this.owner = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        owner.RemoveActiveBullet(this);

        if (collision.gameObject.tag == "Player")
        {
            //decrease health and stuff
        }

        Destroy(gameObject);
    }
}
