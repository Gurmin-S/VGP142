using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int baseCoinValue = 1; // Base value of the coin
    public int valueIncreasePerCollection = 1; // Amount by which the coin value increases with each collection
    private int currentCoinValue;

    [Tooltip("Time before the coin respawns after being collected.")]
    public float respawnTime = 5f;

    private bool isCollected = false; // To track if the coin has been collected
    private MeshRenderer meshRenderer;
    private Collider coinCollider;

    void Start()
    {
        currentCoinValue = baseCoinValue; // Start with the base coin value
        meshRenderer = GetComponent<MeshRenderer>();
        coinCollider = GetComponent<Collider>();

        if (meshRenderer == null || coinCollider == null)
        {
            Debug.LogError("Coin is missing required components.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the coin and if it hasn't been collected yet
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true; // Mark the coin as collected

            // Increase the player's total coin count by the current coin's value
            PlayerCoins playerCoins = other.GetComponent<PlayerCoins>();
            if (playerCoins != null)
            {
                playerCoins.AddCoins(currentCoinValue);
            }

            // Increase the value of subsequent coins
            currentCoinValue += valueIncreasePerCollection;

            // Start the respawn process
            StartCoroutine(RespawnCoin());
        }
    }

    private IEnumerator RespawnCoin()
    {
        // Disable the coin visuals and collider (simulate collection)
        meshRenderer.enabled = false;
        coinCollider.enabled = false;

        // Wait for the respawn time
        yield return new WaitForSeconds(respawnTime);

        // Re-enable the coin visuals and collider for collection again
        meshRenderer.enabled = true;
        coinCollider.enabled = true;

        // Reset the coin value when it respawns
        currentCoinValue = baseCoinValue;

        // Mark the coin as uncollected
        isCollected = false;
    }
}
