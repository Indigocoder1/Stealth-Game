using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthColor;
    [SerializeField] Gradient healthColorGradient;

    private int currentHealth;

    private void Start()
    {
        currentHealth = health;

        healthBar.maxValue = 1;
        healthBar.minValue = 0;

        float normalizedHealth = (float)currentHealth / health;
        healthColor.color = healthColorGradient.Evaluate(normalizedHealth);
        healthBar.value = normalizedHealth;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O))
        {
            Damage(10);
        }
#endif
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        float normalizedHealth = (float)currentHealth / health;

        healthColor.color = healthColorGradient.Evaluate(normalizedHealth);
        healthBar.value = normalizedHealth;
        Debug.Log("Damage Taken: " + damage + "hp lost;");
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public float GetHealthNormalized()
    {
        return (float)currentHealth / health;
    }
}
