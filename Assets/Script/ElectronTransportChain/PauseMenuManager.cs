using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Scene Settings")]
    [SerializeField] private string mapSceneName = "MapScene"; // Set the scene name for Return to Map

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause when the Escape key is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // Call this method to pause the game.
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        Debug.Log("Game paused, timeScale = " + Time.timeScale);
        isPaused = true;
    }


    // Call this method to resume the game.
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
        Time.timeScale = 1f;  // Resume normal time.
        isPaused = false;
    }

    // Button callback: Return to map scene.
    public void ReturnToMap()
    {
        // Resume time before changing scenes.
        Time.timeScale = 1f;
        SceneManager.LoadScene(mapSceneName);
    }

    // Button callback: Open the settings panel.
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Button callback: Close the settings panel.
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
