using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // For accessing the UI components

public class Actor : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;

    // Serialized field to allow assignment in the Unity Inspector
    [SerializeField] private Slider healthSlider;

    void Awake()
    {
        currentHealth = maxHealth;

        // Ensure the slider is set to the max health at the start
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Ensure the current health doesn't go below 0
        if (currentHealth < 0)
            currentHealth = 0;

        // Update the slider with the current health value
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        // Death function
        // TEMPORARY: Destroy Object
        Destroy(gameObject);
    }
}
