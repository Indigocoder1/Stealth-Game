using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionScript : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = bullet.transform.position.x;
        float y = bullet.transform.position.z;
        if (x < -30 || x > 35 || y < -30 || y > 30)
        {
            Destroy(bullet);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "humanoid")
        {
            //decrease health and stuff
        }
        Destroy(bullet);
    }
}
