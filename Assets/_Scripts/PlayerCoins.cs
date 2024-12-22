using UnityEngine;
using UnityEngine.UI; // Add this for working with UI elements

public class PlayerCoins : MonoBehaviour
{
    [Header("Player Coin Settings")]
    public int totalCoins = 0; // The player's total coin count
    public Slider coinSlider; // Reference to the UI Slider component
    public int maxCoins = 100; // The maximum value of the coin slider (adjustable)

    // Method to add coins to the player's total
    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log("Coins collected: " + amount + " | Total coins: " + totalCoins);

        // Update the slider value based on the total coins, scaling it to the maxCoins
        UpdateCoinSlider();
    }

    // Method to update the slider value
    private void UpdateCoinSlider()
    {
        if (coinSlider != null)
        {
            // Scale the total coins to the range of the slider
            float normalizedCoins = (float)totalCoins / maxCoins;
            coinSlider.value = Mathf.Clamp01(normalizedCoins); // Ensure the value stays between 0 and 1
        }
    }

    private void Start()
    {
        // Ensure the slider starts with the correct value
        UpdateCoinSlider();
    }
}
