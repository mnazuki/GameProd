using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Scene Settings")]
    [SerializeField] private string mapSceneName = "MapScene"; // Scene name for Return to Map

    [Header("Audio Settings")]
    [Tooltip("AudioSource used to play the button click sound")]
    [SerializeField] private AudioSource buttonAudioSource;
    [Tooltip("AudioClip to play when a button is clicked")]
    [SerializeField] private AudioClip buttonClickSound;

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause with the Escape key.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // Pauses the game by showing the pause menu.
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        // Ensure the settings panel is closed when pausing.
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        Time.timeScale = 0f;
        Debug.Log("Game paused, timeScale = " + Time.timeScale);
        isPaused = true;
    }

    // Resumes the game by hiding the pause menu.
    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Called when the Return to Map button is clicked.
    public void ReturnToMap()
    {
        // Play click sound then load the map scene after the sound finishes.
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
            StartCoroutine(LoadMapSceneAfterSound());
        }
        else
        {
            // Fallback immediately if sound settings are missing.
            Time.timeScale = 1f;
            SceneManager.LoadScene(mapSceneName);
        }
    }

    // Coroutine to wait for the button click sound before loading the map scene.
    private System.Collections.IEnumerator LoadMapSceneAfterSound()
    {
        yield return new WaitForSeconds(buttonClickSound.length);
        Time.timeScale = 1f; // Resume normal time before loading.
        SceneManager.LoadScene(mapSceneName);
    }

    // Opens the settings panel and plays a click sound.
    public void OpenSettings()
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Closes the settings panel and plays a click sound.
    public void CloseSettings()
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
