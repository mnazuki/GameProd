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

    [Header("Main Menu Options")]
    [Tooltip("Name of the scene to load when the player clicks Continue on the main menu (progress is preserved)")]
    public string continueSceneName = "MainMenu_Continue";

    [Tooltip("Name of the scene to load when the player clicks New Game on the main menu (progress is reset)")]
    public string newGameSceneName = "MainMenu_NewGame";

    [Header("Audio Settings")]
    [Tooltip("AudioSource to play the button click sound")]
    public AudioSource buttonAudioSource;
    [Tooltip("AudioClip to play when a button is clicked")]
    public AudioClip buttonClickSound;

    /// <summary>
    /// Called when the player clicks the Exit button that resets PlayerPrefs.
    /// This method clears all saved PlayerPrefs, plays the click sound, and then loads the exit scene.
    /// </summary>
    public void ExitGameAndReset()
    {
        // Reset all PlayerPrefs.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been reset. Exiting to: " + exitSceneNameReset);
        StartCoroutine(LoadSceneAfterSound(exitSceneNameReset));
    }

    /// <summary>
    /// Called when the player clicks the Exit button that does not reset PlayerPrefs.
    /// This method plays the click sound then loads the primary exit scene without altering saved data.
    /// </summary>
    public void ExitGameNoReset()
    {
        Debug.Log("Exiting to: " + exitSceneNameNoReset + " without resetting PlayerPrefs.");
        StartCoroutine(LoadSceneAfterSound(exitSceneNameNoReset));
    }

    /// <summary>
    /// Called when the player clicks an alternate exit button that does not reset PlayerPrefs.
    /// This method plays the click sound then loads an alternate exit scene.
    /// </summary>
    public void ExitGameNoResetAlt()
    {
        Debug.Log("Exiting to alternate scene: " + exitSceneNameNoResetAlt + " without resetting PlayerPrefs.");
        StartCoroutine(LoadSceneAfterSound(exitSceneNameNoResetAlt));
    }

    /// <summary>
    /// Called when the player clicks the Retry button.
    /// This method reloads the current scene without resetting PlayerPrefs after playing the click sound.
    /// </summary>
    public void RetryGame()
    {
        Debug.Log("Reloading current scene without resetting PlayerPrefs.");
        StartCoroutine(LoadSceneAfterSound(SceneManager.GetActiveScene().buildIndex));
    }

    /// <summary>
    /// Called when the player clicks the Continue button on the main menu.
    /// This method preserves all saved progress, plays the click sound, and loads the continue scene.
    /// </summary>
    public void ContinueGame()
    {
        Debug.Log("Continuing game with saved progress. Loading scene: " + continueSceneName);
        StartCoroutine(LoadSceneAfterSound(continueSceneName));
    }

    /// <summary>
    /// Called when the player clicks the New Game button on the main menu.
    /// This method resets all saved progress (PlayerPrefs), plays the click sound, and loads the new game scene.
    /// </summary>
    public void NewGame()
    {
        Debug.Log("Starting a new game – resetting PlayerPrefs and loading scene: " + newGameSceneName);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        StartCoroutine(LoadSceneAfterSound(newGameSceneName));
    }

    /// <summary>
    /// Helper coroutine that plays the button click sound (if available), waits for the clip to finish, then loads the scene.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    private System.Collections.IEnumerator LoadSceneAfterSound(string sceneName)
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
            yield return new WaitForSeconds(buttonClickSound.length);
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Overload for loading a scene by build index.
    /// </summary>
    /// <param name="buildIndex">Build index of the scene to load.</param>
    private System.Collections.IEnumerator LoadSceneAfterSound(int buildIndex)
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
            yield return new WaitForSeconds(buttonClickSound.length);
        }
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// (For testing in the Editor) When exiting Play Mode, clear PlayerPrefs.
    /// This code runs only in the Unity Editor.
    /// </summary>
    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        Debug.Log("Exiting play mode or game — resetting PlayerPrefs for testing.");
        PlayerPrefs.DeleteAll();
#endif
    }
}
