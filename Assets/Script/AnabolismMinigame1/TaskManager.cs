using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    public Text scoreText;
    public int playerScore = 0;
    public GameObject gameOverScreen;
    public GameObject nextRoundScreen; // Reference to the Next Round UI panel
    public Button continueButton; // Reference to the Continue button

    public CollectionScript collectionScript;
    public MoleculeSpawner moleculeSpawner;

    private void Start()
    {
        moleculeSpawner = FindObjectOfType<MoleculeSpawner>(); // Ensure it's assigned
        nextRoundScreen.SetActive(false); // Hide the screen at the start
        continueButton.onClick.AddListener(ContinueToNextRound); // Assign button click event
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void nextRound()
    {
        collectionScript.ResetCollection(); // Reset collected molecules
        nextRoundScreen.SetActive(true); // Show next round screen

        ResetScore(); // Reset the score

        int currentRound = collectionScript.gameRound;
        Debug.Log("Next round started: " + currentRound);


        moleculeSpawner.UpdateSpawnDelay(currentRound); // Adjust spawn delay
        moleculeSpawner.RestartSpawning(); // Resume spawning
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

    private void ClearExistingMolecules()
    {
        GameObject[] molecules = GameObject.FindGameObjectsWithTag("Molecule");
        foreach (GameObject molecule in molecules)
        {
            Destroy(molecule);
        }
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