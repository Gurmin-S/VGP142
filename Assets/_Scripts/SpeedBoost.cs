using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedIncrease = 2f; // The amount of speed to add
    public float boostDuration = 2f; // Duration of the boost (2 seconds)

    private Renderer objectRenderer; // Reference to the object's renderer
    private bool isActivated = false; // Flag to track if the boost has been activated

    private void Start()
    {
        // Get the Renderer component on the object
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the speed boost and the boost has not been activated
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true; // Mark the boost as activated

            // Start the speed boost effect
            StartCoroutine(ActivateSpeedBoost());

            // Make the object invisible by disabling its renderer
            if (objectRenderer != null)
            {
                objectRenderer.enabled = false;
            }
        }
    }

    private IEnumerator ActivateSpeedBoost()
    {
        // Increase speed for the player by modifying the static walkSpeed variable
        PlayerController.walkSpeed *= speedIncrease;

        // Wait for the duration of the boost
        yield return new WaitForSeconds(boostDuration);

        // Reset the player's speed after the boost duration
        PlayerController.walkSpeed /= speedIncrease;

        // Destroy the speed boost object after the effect ends
        Destroy(gameObject);
    }
}
