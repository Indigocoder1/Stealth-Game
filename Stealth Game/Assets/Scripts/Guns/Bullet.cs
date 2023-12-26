using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform owner;

    public void SetOwner(Transform owner)
    {
        this.owner = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //decrease health and stuff
        }

        Destroy(gameObject);
    }
}
