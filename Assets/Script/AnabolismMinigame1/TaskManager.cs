using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    public Text scoreText;
    public int playerScore = 0;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject nextRoundScreen;
    public Button continueButton;

    public CollectionScript collectionScript;
    public MoleculeSpawner moleculeSpawner;
    public AudioSource bgmsrc;
    public AudioClip bgm;
    public GameObject d3;
    public GameObject d4;

    private void Start()
    {
        bgmsrc.clip = bgm;
        bgmsrc.loop = true;
        bgmsrc.Play();

        moleculeSpawner = FindObjectOfType<MoleculeSpawner>();
        collectionScript = FindObjectOfType<CollectionScript>();
        if (collectionScript == null)
        {
            Debug.LogError("collectionScript is NULL in TaskManager!");
            return;
        }

        nextRoundScreen.SetActive(false);
        continueButton.onClick.AddListener(ContinueToNextRound); 

        moleculeSpawner.isGameFinished = false;
        moleculeSpawner.RestartSpawning();

    }

    void Update()
    {
        if (d4 == null){
        Debug.Log("Game Won! Showing Win Screen.");
            winScreen.SetActive(true); // Show the win screen
            Time.timeScale = 0f;
        }
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        
    }

    public void nextRound()
    {
        Debug.Log("nextRound() called!");

        if (collectionScript != null)
        {
            collectionScript.ResetCollection(); // Reset molecule collection
        }
        else
        {
            Debug.LogError("collectionScript is NULL in nextRound!");
        }

        int currentRound = collectionScript.gameRound;

        if (currentRound > 1)
        {
            if (!d4.activeInHierarchy)
            {
                d4.SetActive(true);
            // Stop further execution
            }
        }

        if (!d3.activeInHierarchy){
            d3.SetActive(true);
        }
        nextRoundScreen.SetActive(true); // Show UI for the next round

        ResetScore();

        Debug.Log("Next round started: " + currentRound);

        moleculeSpawner.UpdateSpawnDelay(currentRound);
        moleculeSpawner.RestartSpawning();
        
    }

    public void ContinueToNextRound()
    {
        Time.timeScale = 1f;
        nextRoundScreen.SetActive(false); // Hide the screen

        // Destroy all currently spawned molecules
        ClearExistingMolecules();

        int currentRound = collectionScript.gameRound;
        Debug.Log("Next round started: " + currentRound);

        moleculeSpawner.UpdateSpawnDelay(currentRound); // Adjust spawn delay
        moleculeSpawner.RestartSpawning(); // Resume spawning
    }

    public void ClearExistingMolecules()
    {
        GameObject[] molecules = GameObject.FindGameObjectsWithTag("Molecule");
        foreach (GameObject molecule in molecules)
        {
            Destroy(molecule);
        }
    }

    public void UpdateTaskText(int sucroseGoal, int lactoseGoal, int maltoseGoal, int dnaGoal,
                           int sucroseCollected, int lactoseCollected, int maltoseCollected, int dnaCollected)
    {
        taskText.text = $"Collect:\n" +
                        $"Sucrose: {sucroseCollected}/{sucroseGoal}\n" +
                        $"Lactose: {lactoseCollected}/{lactoseGoal}\n" +
                        $"Maltose: {maltoseCollected}/{maltoseGoal}\n" +
                        $"DNA: {dnaCollected}/{dnaGoal}";
    }

    public void AddScore()
    {
        playerScore++;
        scoreText.text = $"Points: {playerScore}";
    }

    public void ResetScore()
    {
        playerScore = 0;
        scoreText.text = $"Points: {playerScore}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quitting game");
    }
}