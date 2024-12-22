using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        // Check if the player has collected all coins
        if (totalCoins >= maxCoins)
        {
            // Load the new scene when all coins are collected
            LoadScene();
        }
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

    // Method to load the scene once all coins are collected
    private void LoadScene()
    {
        // Optionally, you can also add a delay or effect here before loading the scene.
        SceneManager.LoadScene(2); // You can change the scene index to the appropriate one for your game
    }

    private void Start()
    {
        // Ensure the slider starts with the correct value
        UpdateCoinSlider();
    }
}
