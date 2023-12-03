using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthImage;
    [SerializeField] Gradient healthColor;

    private int currentHealth;

    private void Start()
    {
        currentHealth = health;

        healthBar.maxValue = 1;
        healthBar.minValue = 0;

        float normalizedHealth = (float)currentHealth / health;
        healthImage.color = healthColor.Evaluate(normalizedHealth);
        healthBar.value = normalizedHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Damage(10);
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        float normalizedHealth = (float)currentHealth / health;

        healthImage.color = healthColor.Evaluate(normalizedHealth);
        healthBar.value = normalizedHealth;

        Debug.Log(normalizedHealth);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
