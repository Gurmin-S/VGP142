using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int baseCoinValue = 1; // Base value of the coin
    public int valueIncreasePerCollection = 1; // Amount by which the coin value increases with each collection
    private int currentCoinValue;

    private bool isCollected = false; // To track if the coin has been collected

    private void Start()
    {
        currentCoinValue = baseCoinValue; // Start with the base coin value
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

            // Destroy the coin object after it has been collected
            Destroy(gameObject);
        }
    }
}
