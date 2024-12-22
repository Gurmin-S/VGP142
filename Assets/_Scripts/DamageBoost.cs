using System.Collections;
using UnityEngine;

public class DamageBoost : MonoBehaviour
{
    [Header("Damage Boost Settings")]
    public int damageIncrease = 1; // The amount of damage to add
    public float boostDuration = 5f; // Duration of the boost (in seconds)

    private bool isActivated = false; // Flag to track if the boost has been activated

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the damage boost and the boost has not been activated
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true; // Mark the boost as activated

            // Start the damage boost effect
            StartCoroutine(ActivateDamageBoost(other.gameObject));

            // Destroy the boost object after activation
            Destroy(gameObject);
        }
    }

    private IEnumerator ActivateDamageBoost(GameObject player)
    {
        // Increase the player's attack damage by modifying the Attack script
        Attack playerAttack = player.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.IncreaseDamage(damageIncrease);
        }

        // Wait for the duration of the boost
        yield return new WaitForSeconds(boostDuration);

        // Reset the player's attack damage to its base value
        if (playerAttack != null)
        {
            playerAttack.ResetDamage();
        }

        // Destroy the damage boost object after the effect ends (optional)
        Destroy(gameObject);
    }
}
