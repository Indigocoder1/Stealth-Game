using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FiringScript : MonoBehaviour
{
    public Transform bullet;
    public Transform bulletHolder;
    public GameObject cameraHolder;
    private PlayerMovement playerMovementScript;
    public float bulletSpeed = 10;
    public float fireRate = 2;
    private float time = 0;
    private bool reset = true;
    // Start is called before the first frame update
    void Start()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && time >= fireRate)
        {
            time = 0;
            Transform bulletTrans = Instantiate(bullet, bulletHolder.position, bullet.rotation);
            Rigidbody bulletRB = bulletTrans.GetComponent<Rigidbody>();
            bulletRB.AddForce(Vector3.forward * bulletSpeed * Time.deltaTime * 100f);
            Quaternion rotation = cameraHolder.transform.rotation;
            rotation.x -= 10;
            cameraHolder.transform.rotation = rotation;
            playerMovementScript.setMovementLockState(true);
            reset = false;
        }
        if (time >= 0.25 && !reset)
        {
            playerMovementScript.setMovementLockState(false);
            reset = true;
        }
        time += Time.deltaTime;
    }
}
