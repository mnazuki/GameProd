using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    public Text scoreText;
    public int playerScore = 0;
    public GameObject gameOverScreen;
    public GameObject nextRoundScreen;
    public Button continueButton;

    public CollectionScript collectionScript;
    public MoleculeSpawner moleculeSpawner;

    private void Start()
    {
        moleculeSpawner = FindObjectOfType<MoleculeSpawner>();
        nextRoundScreen.SetActive(false);
        continueButton.onClick.AddListener(ContinueToNextRound); 

        moleculeSpawner.isGameFinished = false;
        moleculeSpawner.RestartSpawning();
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

        nextRoundScreen.SetActive(true); // Show UI for the next round
        ResetScore(); // Reset score

        int currentRound = collectionScript.gameRound;
        Debug.Log("Next round started: " + currentRound);

        moleculeSpawner.UpdateSpawnDelay(currentRound);
        moleculeSpawner.RestartSpawning();
    }

    private void ContinueToNextRound()
    {
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
        taskText.text = $"Collect: Sucrose {sucroseCollected}/{sucroseGoal}, " +
                        $"Lactose {lactoseCollected}/{lactoseGoal}, " +
                        $"Maltose {maltoseCollected}/{maltoseGoal}, " +
                        $"DNA {dnaCollected}/{dnaGoal}";
    }

    public void AddScore()
    {
        playerScore++;
        scoreText.text = playerScore.ToString();
    }

    public void ResetScore()
    {
        playerScore = 0;
        scoreText.text = playerScore.ToString();
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