using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionScript : MonoBehaviour
{
    [SerializeField] private int sucroseCollected;
    [SerializeField] private int lactoseCollected;
    [SerializeField] private int maltoseCollected;
    [SerializeField] private int dnaCollected;
    [SerializeField] private int sucroseGoal;
    [SerializeField] private int lactoseGoal;
    [SerializeField] private int maltoseGoal;
    [SerializeField] private int dnaGoal;

    public TaskManager taskManager;
    public MoleculeSpawner moleculeSpawner;
    private PlayerHealth playerHealth;

    [SerializeField] private bool gameEnded = false;
    public int gameRound = 0;
    public GameObject gameOverPanel;
    public AudioSource bgm;
    [Header("Dialogue")]
    public GameObject d2;

    [Header("Win Reporting Settings")]
    [Tooltip("Unique index for this minigame (for saving completion state, e.g., 1, 2, etc.)")]
    public int minigameIndex = 1;
    [Tooltip("ATP reward for completing the minigame (e.g., 50)")]
    public int atpReward = 50;
    private bool winReported = false;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;

        if (taskManager == null)
        {
            taskManager = FindObjectOfType<TaskManager>();
            Debug.Log("taskManager was null, assigned via FindObjectOfType.");
        }

        ResetCollection();

        moleculeSpawner.UpdateSpawnDelay(gameRound);

        gameRound = 0;

        Debug.Log($"Starting game with: Sucrose {sucroseCollected}/{sucroseGoal}, " +
          $"Lactose {lactoseCollected}/{lactoseGoal}, " +
          $"Maltose {maltoseCollected}/{maltoseGoal}, " +
          $"DNA {dnaCollected}/{dnaGoal}");
    }

    private void Update()
    {
        if (d2 == null)
        {
            bgm.Stop();
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (sucroseCollected > 0 || lactoseCollected > 0 || maltoseCollected > 0 || dnaCollected > 0)
            {
                taskManager.AddScore();
            }
            CheckForRoundCompletion();
        }
    }

    void CheckForRoundCompletion()
    {
        if (sucroseGoal == 0 || lactoseGoal == 0 || maltoseGoal == 0 || dnaGoal == 0)
        {
            Debug.LogWarning("Skipping round completion check due to zero goals.");
            return;
        }

        Debug.Log($"Checking completion: Sucrose {sucroseCollected}/{sucroseGoal}, " +
                  $"Lactose {lactoseCollected}/{lactoseGoal}, " +
                  $"Maltose {maltoseCollected}/{maltoseGoal}, " +
                  $"DNA {dnaCollected}/{dnaGoal}");

        if (!gameEnded &&
            sucroseCollected >= sucroseGoal &&
            lactoseCollected >= lactoseGoal &&
            maltoseCollected >= maltoseGoal &&
            dnaCollected >= dnaGoal)
        {
            gameEnded = true;
            Debug.Log("Game " + gameRound + " completed!");

            // ----- WIN REPORTING CODE START -----
            if (!winReported)
            {
                // Mark the minigame as completed using a unique key.
                PlayerPrefs.SetInt("MinigameCompleted_" + minigameIndex, 1);
                // Award ATP points.
                int currentATP = PlayerPrefs.GetInt("ATP", 0);
                PlayerPrefs.SetInt("ATP", currentATP + atpReward);
                // Save changes.
                PlayerPrefs.Save();
                winReported = true;
            }
            // ----- WIN REPORTING CODE END -----

            moleculeSpawner.isGameFinished = true;
            taskManager.winScreen.SetActive(true); // Show the win screen
            Time.timeScale = 0f;
            StopCoroutine(moleculeSpawner.SpawnMolecules());
        }
    }

    public void ResetCollection()
    {
        sucroseCollected = 0;
        lactoseCollected = 0;
        maltoseCollected = 0;
        dnaCollected = 0;
        gameEnded = false;

        sucroseGoal = Random.Range(1, 4);
        lactoseGoal = Random.Range(1, 4);
        maltoseGoal = Random.Range(1, 4);
        dnaGoal = Random.Range(1, 4);

        Debug.Log($"New Goals - Sucrose: {sucroseGoal}, Lactose: {lactoseGoal}, Maltose: {maltoseGoal}, DNA: {dnaGoal}");

        taskManager.UpdateTaskText(sucroseGoal, lactoseGoal, maltoseGoal, dnaGoal,
                                   sucroseCollected, lactoseCollected, maltoseCollected, dnaCollected);

        moleculeSpawner.isGameFinished = false;
        moleculeSpawner.RestartSpawning();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string moleculeName = collision.gameObject.name;

        if (moleculeName.Contains("Adenine+Thymine") || moleculeName.Contains("Guanine+Cytosine"))
        {
            Debug.Log($"Cannot collect: {moleculeName}");
            playerHealth.health--;
            playerHealth.UpdateHeartsUI();
            Destroy(collision.gameObject);
            return;
        }

        if (CombinationScript.moleculeInstances.Contains(collision.gameObject))
        {
            Debug.Log("Collected: " + collision.gameObject.name);

            if (collision.gameObject.name.Contains("Sucrose"))
                sucroseCollected++;
            else if (collision.gameObject.name.Contains("Lactose"))
                lactoseCollected++;
            else if (collision.gameObject.name.Contains("Maltose"))
                maltoseCollected++;
            else if (collision.gameObject.name.Contains("DNA"))
                dnaCollected++;

            taskManager.AddScore();
            CombinationScript.moleculeInstances.Remove(collision.gameObject);
            Destroy(collision.gameObject);

            taskManager.UpdateTaskText(sucroseGoal, lactoseGoal, maltoseGoal, dnaGoal,
                                       sucroseCollected, lactoseCollected, maltoseCollected, dnaCollected);

            CheckForRoundCompletion();
        }
        else
        {
            playerHealth.health--;
            playerHealth.UpdateHeartsUI();

            if (playerHealth.health == 0)
            {
                if (!d2.activeInHierarchy)
                {
                    d2.SetActive(true);
                }
            }

            Destroy(collision.gameObject);
            Debug.Log("Not a valid object to collect.");
        }
    }
}
