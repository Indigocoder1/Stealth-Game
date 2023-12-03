using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private Text healthP;
    public float Damage(int damageAmount)
    {
        health -= damageAmount;


        if (health < 25)
        {
            healthP.color = Color.red;
        }
        else if (health < 50)
        {
            healthP.color = new Color(255, 182, 0);
        }
        else if (health < 75)
        {
            healthP.color = new Color(255, 200, 0);
        }

        for (int i = 0; i < health/4; i++)
        {
            healthP.text = healthP.text + "|";
        }

        Debug.Log(health);

        return health;
    }
}
