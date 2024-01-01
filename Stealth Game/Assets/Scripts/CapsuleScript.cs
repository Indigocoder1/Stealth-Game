using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(UnityEngine.Collider collider)
    {
        GameObject thing = collider.gameObject;
        if (thing.tag == "Bullet")
        {
            Debug.Log("Bullet is of Team: " + thing.GetComponent<TaserBullet>().GetTeam() + ", Player is of Team: " + player.GetComponent<TeamScript>().getTeam());
            if (thing.GetComponent<TaserBullet>().GetTeam() != player.GetComponent<TeamScript>().getTeam())
            {
                player.GetComponent<PlayerHealth>().Damage(thing.GetComponent<TaserBullet>().GetDamage());
            }
        }
    }
}
