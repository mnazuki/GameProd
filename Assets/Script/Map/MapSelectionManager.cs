using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MinigameButtonInfo
{
    [Header("UI References")]
    public Button button;           // Button to click for the minigame.
    public Image buttonImage;       // The image component whose sprite we change.

    [Header("Sprites")]
    public Sprite defaultSprite;    // The sprite when the minigame is not yet completed.
    public Sprite completedSprite;  // The sprite to display once the minigame is won.

    [Header("Minigame Settings")]
    [Tooltip("Name of the scene to load when this minigame is selected")]
    public string sceneToLoad;      // The scene name to load when this button is clicked.
    [Tooltip("ATP required to unlock this minigame")]
    public int ATPRequirement;      // The ATP points required to be able to click this minigame.
    [Tooltip("Unique index for this minigame (used for saving completion state)")]
    public int minigameIndex;       // A unique number representing this minigame; use this as part of the PlayerPrefs key.
}

public class MapSelectionManager : MonoBehaviour
{
    [Header("Minigame Buttons Setup")]
    public List<MinigameButtonInfo> minigameButtons; // List of all minigame button information.

    [Header("ATP UI")]
    public TextMeshProUGUI atpCounterText;      // A UI Text element that displays the current ATP points.

    [Header("Audio Settings")]
    [Tooltip("AudioSource to play the button click sound")]
    public AudioSource buttonAudioSource;
    [Tooltip("AudioClip to play when a minigame button is clicked")]
    public AudioClip buttonClickSound;

    private int currentATP;

    private void Start()
    {
        // Load the player's current ATP; default is 0 if not already set.
        currentATP = PlayerPrefs.GetInt("ATP", 0);
        UpdateATPCounterUI();

        // Set up each minigame button based on saved completion state and ATP value.
        SetupMinigameButtons();
    }

    /// <summary>
    /// Sets the interactability and visuals of each minigame button based on completion status and ATP.
    /// </summary>
    private void SetupMinigameButtons()
    {
        foreach (MinigameButtonInfo gameButton in minigameButtons)
        {
            // Determine if this minigame has been completed.
            // We use a key like "MinigameCompleted_1" where the number is the minigameIndex.
            int completed = PlayerPrefs.GetInt("MinigameCompleted_" + gameButton.minigameIndex, 0);
            if (completed == 1)
            {
                // Change the button's sprite to show that this minigame is already completed.
                if (gameButton.buttonImage != null && gameButton.completedSprite != null)
                {
                    gameButton.buttonImage.sprite = gameButton.completedSprite;
                }
            }
            else
            {
                // Otherwise, ensure the default sprite is used.
                if (gameButton.buttonImage != null && gameButton.defaultSprite != null)
                {
                    gameButton.buttonImage.sprite = gameButton.defaultSprite;
                }
            }

            // Enable or disable the button based on whether the player has enough ATP.
            if (currentATP >= gameButton.ATPRequirement)
            {
                gameButton.button.interactable = true;
            }
            else
            {
                gameButton.button.interactable = false;
            }

            // Remove any previous listeners (to avoid duplicate calls) and add the click listener.
            gameButton.button.onClick.RemoveAllListeners();
            // We use a lambda here to pass the gameButton info to the OnMinigameButtonClicked method.
            gameButton.button.onClick.AddListener(() => OnMinigameButtonClicked(gameButton));
        }
    }

    /// <summary>
    /// Called when a minigame button is clicked.
    /// Checks ATP, plays the click sound, then loads the respective scene after waiting the clip duration.
    /// </summary>
    /// <param name="gameButton">The minigame button data that was clicked.</param>
    private void OnMinigameButtonClicked(MinigameButtonInfo gameButton)
    {
        // Safety check: make sure the player has enough ATP.
        if (currentATP >= gameButton.ATPRequirement)
        {
            // Play click sound then load scene.
            StartCoroutine(LoadSceneAfterSound(gameButton.sceneToLoad));
        }
        else
        {
            // Feedback when ATP is insufficient.
            Debug.Log("Not enough ATP to play this minigame!");
        }
    }

    /// <summary>
    /// Updates the UI text that shows the current ATP value.
    /// </summary>
    private void UpdateATPCounterUI()
    {
        if (atpCounterText != null)
        {
            atpCounterText.text = "ATP: " + currentATP;
        }
    }

    /// <summary>
    /// Call this method when returning to the map scene (for example, after finishing a minigame)
    /// to refresh the UI. It reloads the ATP value and updates the button states.
    /// </summary>
    public void RefreshMapUI()
    {
        currentATP = PlayerPrefs.GetInt("ATP", 0);
        UpdateATPCounterUI();
        SetupMinigameButtons();
    }

    /// <summary>
    /// When exiting the game or stopping play mode in the Editor, clear saved data for testing.
    /// </summary>
    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        Debug.Log("Exiting play mode or game — resetting PlayerPrefs for testing.");
        PlayerPrefs.DeleteAll();
#endif
    }

    /// <summary>
    /// Coroutine that plays the button click sound (if available) and then loads the scene after the sound finishes.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAfterSound(string sceneName)
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
            yield return new WaitForSeconds(buttonClickSound.length);
        }
        SceneManager.LoadScene(sceneName);
    }
}
