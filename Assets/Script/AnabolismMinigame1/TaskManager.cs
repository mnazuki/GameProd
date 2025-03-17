using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    public Text scoreText;
    public int playerScore = 0;
    public GameObject gameOverScreen;

    public CollectionScript collectionScript;

    public MoleculeSpawner moleculeSpawner;

    private void Start()
    {
        moleculeSpawner = FindObjectOfType<MoleculeSpawner>(); // Ensure it's assigned
    }


    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void nextRound()
    {
        collectionScript.ResetCollection(); // Reset collected molecules

        int currentRound = collectionScript.gameRound;
        Debug.Log("Next round started: " + currentRound);

        if (moleculeSpawner == null)
        {
            Debug.LogError("MoleculeSpawner is NULL! Check if it's assigned in TaskManager.");
            return;
        }

        moleculeSpawner.UpdateSpawnDelay(currentRound); // Adjust spawn delay
        moleculeSpawner.RestartSpawning(); // Resume spawning

    }

    public void AddScore()
    {
            playerScore++;
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
