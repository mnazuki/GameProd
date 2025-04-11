using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIConveyor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] GameObject gameOverPlane, winPlane;
    [SerializeField] Button resetButton, nextSceneButton, gameOverRetryButton;
    private PlayerHealth playerHealthSC;
    private MoleculeConveyor moleculeConveyor;

    [Header("Win Reporting Settings")]
    [Tooltip("Unique index for this minigame (for saving completion state, e.g., 1, 2, etc.)")]
    public int minigameIndex = 1;
    [Tooltip("ATP reward for completing the minigame (e.g., 50)")]
    public int atpReward = 50;
    private bool winReported = false;

    private void Start()
    {
        playerHealthSC = FindFirstObjectByType<PlayerHealth>();
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();
    }

    private void Update()
    {
        scoreTxt.text = "Score: " + playerHealthSC.score + "/5";

        if (playerHealthSC.isGameOver)
        {
            gameOverPlane.SetActive(true);
            Time.timeScale = 0.01f;
        }

        if (playerHealthSC.score == playerHealthSC.scoreMax)
        {
            // Only perform win reporting once.
            if (!winReported)
            {
                // Mark the minigame as completed using a unique key.
                PlayerPrefs.SetInt("MinigameCompleted_" + minigameIndex, 1);

                // Award ATP points.
                int currentATP = PlayerPrefs.GetInt("ATP", 0);
                PlayerPrefs.SetInt("ATP", currentATP + atpReward);

                // Save the changes.
                PlayerPrefs.Save();

                winReported = true;
            }

            winPlane.SetActive(true);
            Time.timeScale = 0.01f;
        }

        // If needed, you can uncomment these to add button listeners.
        // gameOverRetryButton.onClick.AddListener(ResetButton);
        // resetButton.onClick.AddListener(ResetButton);
        // nextSceneButton.onClick.AddListener(NextMinigameButton);
    }

    public void ResetButton()
    {
        Debug.Log("is Reset");

        // Reset score and health for a fresh start.
        playerHealthSC.score = 0;
        playerHealthSC.health = playerHealthSC.numberOfHearts;
        playerHealthSC.UpdateHeartsUI();
        playerHealthSC.isGameOver = false;

        gameOverPlane.SetActive(false);
        winPlane.SetActive(false);

        // Restore normal time scale.
        Time.timeScale = 1;

        // Reset win reporting so that if the player plays again, win reporting will happen anew.
        winReported = false;
    }

    public void NextMinigameButton()
    {
        // Example: load the next scene.
        // SceneManager.LoadScene("NextMinigameSceneName");
    }
}
