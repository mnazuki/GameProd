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
            gameRound++;
            Debug.Log("Round " + gameRound + " completed!");

            moleculeSpawner.isGameFinished = true;
            StopCoroutine(moleculeSpawner.SpawnMolecules());

            taskManager.nextRound();
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
            return; // Stop further execution
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
                gameOverPanel.SetActive(true);
                Time.timeScale = 0f;
            }

            Destroy(collision.gameObject);
            Debug.Log("Not a valid object to collect.");
        }
    }
}
