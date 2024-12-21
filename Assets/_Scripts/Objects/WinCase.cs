using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnDestroy : MonoBehaviour
{
    // The index of the scene to load
    public int sceneIndex = 2;

    // This method is called when the GameObject is destroyed
    void OnDestroy()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the scene with the specified index
        SceneManager.LoadScene(sceneIndex);
    }
}
