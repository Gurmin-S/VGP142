using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Reference to the Play button
    public Button playButton;

    // Reference to the Exit button
    public Button exitButton;

    void Start()
    {
        // Assign the PlayButtonClick method to the Play button's onClick event
        playButton.onClick.AddListener(PlayButtonClick);

        // Assign the ExitButtonClick method to the Exit button's onClick event
        exitButton.onClick.AddListener(ExitButtonClick);
    }

    // Method called when the Play button is clicked
    void PlayButtonClick()
    {
        // Load the scene with index 1 (assuming Scene 1 is at index 1 in the Build Settings)
        SceneManager.LoadScene(1);
    }

    // Method called when the Exit button is clicked
    void ExitButtonClick()
    {
        // Exit play mode in the editor or quit the application in a built game
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
