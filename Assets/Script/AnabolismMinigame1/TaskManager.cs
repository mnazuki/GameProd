using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    public Text scoreText;
    public Text timerText;
    public int playerScore = 0;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject nextRoundScreen;
    public Button continueButton;

    public float timerMinutes = 2f;
    private float timeRemaining;
    private bool timerRunning = true;

    public CollectionScript collectionScript;
    public MoleculeSpawner moleculeSpawner;
    public AudioSource bgmsrc;
    public AudioClip bgm;
    public GameObject d1;
    public GameObject d2;
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

        timeRemaining = timerMinutes * 60f; // Convert minutes to seconds

        nextRoundScreen.SetActive(false);
        //continueButton.onClick.AddListener(ContinueToNextRound); 

    }

    void Update()
    {
        if (d4 == null){
        Debug.Log("Game Won! Showing Win Screen.");
            winScreen.SetActive(true); // Show the win screen
            Time.timeScale = 0f;
        }

        HandleTimer();
    }

    void HandleTimer()
    {
        // Pause timer if d1 or d2 is open
        if ((d1 != null && d1.activeInHierarchy) || (d2 != null && d2.activeInHierarchy))
        {
            return; // Skip timer update
        }

        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                gameOver();
            }

            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        d2.SetActive(true);
    }

    //public void nextRound()
    //{
    //    Debug.Log("nextRound() called!");

    //    if (collectionScript != null)
    //    {
    //        collectionScript.ResetCollection(); // Reset molecule collection
    //    }
    //    else
    //    {
    //        Debug.LogError("collectionScript is NULL in nextRound!");
    //    }

    //    int currentRound = collectionScript.gameRound;

    //    if (currentRound > 1)
    //    {
    //        if (!d4.activeInHierarchy)
    //        {
    //            d4.SetActive(true);
    //        // Stop further execution
    //        }
    //    }

    //    if (!d3.activeInHierarchy){
    //        d3.SetActive(true);
    //    }
    //    nextRoundScreen.SetActive(true); // Show UI for the next round

    //    ResetScore();

    //    Debug.Log("Next round started: " + currentRound);

    //    moleculeSpawner.UpdateSpawnDelay(currentRound);
    //    moleculeSpawner.RestartSpawning();

    //}

    //public void ContinueToNextRound()
    //{
    //    Time.timeScale = 1f;
    //    nextRoundScreen.SetActive(false); // Hide the screen

    //    // Destroy all currently spawned molecules
    //    ClearExistingMolecules();

    //    int currentRound = collectionScript.gameRound;
    //    Debug.Log("Next round started: " + currentRound);

    //    moleculeSpawner.UpdateSpawnDelay(currentRound); // Adjust spawn delay
    //    moleculeSpawner.RestartSpawning(); // Resume spawning
    //}

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