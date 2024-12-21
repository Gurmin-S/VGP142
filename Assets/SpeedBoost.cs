using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedIncrease = 2f; // The amount of speed to add
    public float boostDuration = 2f; // Duration of the boost (2 seconds)

    [Header("Player Reference")]
    [SerializeField] private PlayerController playerMovement; // Reference to the player's movement script

    private Renderer objectRenderer; // Reference to the object's renderer

    private void Start()
    {
        // Automatically find the PlayerController attached to the Player object
        if (playerMovement == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerController>();
                if (playerMovement == null)
                {
                    Debug.LogError("PlayerController script not found on Player object!");
                }
            }
            else
            {
                Debug.LogError("Player object with tag 'Player' not found!");
            }
        }

        // Get the Renderer component on the object
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the speed boost
        if (other.CompareTag("Player"))
        {
            // Ensure the playerMovement script is assigned
            if (playerMovement != null)
            {
                // Start the speed boost effect
                StartCoroutine(ActivateSpeedBoost());
            }
            else
            {
                Debug.LogWarning("PlayerMovement script is not assigned in the SpeedBoost object.");
            }

            // Make the object invisible by disabling its renderer
            if (objectRenderer != null)
            {
                objectRenderer.enabled = false;
            }
        }
    }

    private IEnumerator ActivateSpeedBoost()
    {
        // Increase speed for the player
        playerMovement.walkSpeed *= speedIncrease;

        // Wait for the duration of the boost
        yield return new WaitForSeconds(boostDuration);

        // Reset the player's speed after the boost duration
        playerMovement.walkSpeed /= speedIncrease;

        // Destroy the speed boost object after the effect ends
        Destroy(gameObject);
    }
}
