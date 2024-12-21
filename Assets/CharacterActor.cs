using System.Collections;
using System.Collections.Generic;  // Import for List
using UnityEngine;
using UnityEngine.UI;

public class CharacterActor : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;

    // Serialized field to allow assignment in the Unity Inspector
    [SerializeField] private Slider healthSlider;

    // Reference to the Animator to trigger the "Hit" animation and manage death state
    [SerializeField] private Animator animator;

    // Bool to check if the actor is dead
    public bool isDead = false;

    // List to hold all scripts to be disabled on death
    [SerializeField] private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();

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
        if (isDead) return; // Prevent damage if the actor is already dead

        currentHealth -= amount;

        // Ensure the current health doesn't go below 0
        if (currentHealth < 0)
            currentHealth = 0;

        // Update the slider with the current health value
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Trigger the "Hit" animation
        if (animator != null)
        {
            animator.SetTrigger("Hit");

            // Optionally reset the trigger manually if you need more control
            ResetHitTrigger();
        }

        // Check for death
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void ResetHitTrigger()
    {
        // This is a coroutine to wait for a frame and then reset the "Hit" trigger
        StartCoroutine(ResetHitTriggerCoroutine());
    }

    private IEnumerator ResetHitTriggerCoroutine()
    {
        // Wait until the next frame before resetting the trigger
        yield return null;

        // Reset the "Hit" trigger after it has been set
        animator.ResetTrigger("Hit");
    }

    void Death()
    {
        if (isDead) return;

        // Set the isDead flag to true
        isDead = true;

        // Update the "Dead" state in the Animator (if applicable)
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Disable the scripts in the disabledScripts list when the actor is dead
        DisableScripts();

        // Any additional death logic could go here
    }

    void DisableScripts()
    {
        // Loop through each script in the disabledScripts list and disable it
        foreach (MonoBehaviour script in disabledScripts)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }
    }
}
