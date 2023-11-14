using UnityEngine;

public class BulletCollisionScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //decrease health and stuff
        }

        Destroy(gameObject);
    }
}
