using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRetryManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load when the player exits and resets PlayerPrefs (e.g., MainMenuReset)")]
    public string exitSceneNameReset = "MainMenu";

    [Tooltip("Name of the scene to load when the player exits without resetting PlayerPrefs (primary)")]
    public string exitSceneNameNoReset = "MainMenu_NoReset";

    [Tooltip("Name of the alternate scene to load when the player exits without resetting PlayerPrefs (alternate)")]
    public string exitSceneNameNoResetAlt = "AlternateMenu";

    /// <summary>
    /// Called when the player clicks the Exit button that resets PlayerPrefs.
    /// This method clears all saved PlayerPrefs and loads the designated exit scene.
    /// </summary>
    public void ExitGameAndReset()
    {
        // Reset all PlayerPrefs.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been reset. Exiting to: " + exitSceneNameReset);

        // Load the designated exit scene.
        SceneManager.LoadScene(exitSceneNameReset);
    }

    /// <summary>
    /// Called when the player clicks the Exit button that does not reset PlayerPrefs.
    /// This method simply loads the primary exit scene without altering saved data.
    /// </summary>
    public void ExitGameNoReset()
    {
        Debug.Log("Exiting to: " + exitSceneNameNoReset + " without resetting PlayerPrefs.");
        SceneManager.LoadScene(exitSceneNameNoReset);
    }

    /// <summary>
    /// Called when the player clicks an alternate exit button that does not reset PlayerPrefs.
    /// This method loads an alternate exit scene.
    /// </summary>
    public void ExitGameNoResetAlt()
    {
        Debug.Log("Exiting to alternate scene: " + exitSceneNameNoResetAlt + " without resetting PlayerPrefs.");
        SceneManager.LoadScene(exitSceneNameNoResetAlt);
    }

    /// <summary>
    /// Called when the player clicks the Retry button.
    /// This method reloads the current scene without resetting PlayerPrefs.
    /// </summary>
    public void RetryGame()
    {
        Debug.Log("Reloading current scene without resetting PlayerPrefs.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
