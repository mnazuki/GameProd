using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject minigamePanel;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI protonCounterText;
    public GameObject gameOverScreen;
    public Button connectButton;
    public Button resetButton;

    [Header("Proton Spawning")]
    public GameObject protonPrefab;
    public GameObject fakeProtonPrefab;
    public Transform protonContainer;
    private List<GameObject> spawnedProtons = new List<GameObject>();

    [Header("Game Variables")]
    public int playerHP = 3;
    public int maxTime = 10;
    private int timeLeft;
    private int protonsClicked = 0;
    private bool isMinigameActive = false;
    private List<ProtonMovement> protonsInBooth = new List<ProtonMovement>();
    private bool isTimerRunning = false;
    private int totalProtonsCollected = 0;
    private int difficultyLevel = 1;
    private int maxRounds = 1;
    private int currentRound = 0;
    private int currentBoothNumber;
    private List<GameObject> movingProtons = new List<GameObject>();
    private Coroutine moveProtonsCoroutine;

    public enum SpeedLevel { Slow, Normal, Fast }

    [Header("Speed Settings for Booth 3")]
    public SpeedLevel booth3Speed = SpeedLevel.Normal;

    void Start()
    {
        gameOverScreen.SetActive(false);
        minigamePanel.SetActive(false);
        hpText.text = "HP: " + playerHP;
        protonCounterText.text = $"Protons: {totalProtonsCollected}/24";

        resetButton.onClick.AddListener(ResetGame);
    }

    public void StartMinigame(List<ProtonMovement> protons, int boothNumber)
    {
        if (isMinigameActive) return;
        isMinigameActive = true;
        currentRound = 0;
        currentBoothNumber = boothNumber;

        protonsInBooth = protons;
        minigamePanel.SetActive(true);
        timeLeft = maxTime;
        protonsClicked = 0;
        timerText.text = "Time: " + timeLeft;

        SetDifficulty(boothNumber);
        StartRound();
    }

    void SetDifficulty(int boothNumber)
    {
        if (boothNumber == 1)
        {
            difficultyLevel = 1;
            maxRounds = 1;
        }
        else if (boothNumber == 2)
        {
            difficultyLevel = 2;
            maxRounds = 2;
        }
        else if (boothNumber == 3)
        {
            difficultyLevel = 3;
            maxRounds = 3;
        }
    }

    void StartRound()
    {
        if (currentRound >= maxRounds)
        {
            EndMinigame();
            return;
        }

        // Stop the previous movement coroutine if it exists
        if (moveProtonsCoroutine != null)
        {
            StopCoroutine(moveProtonsCoroutine);
            moveProtonsCoroutine = null;
        }

        // Reset round-specific state
        protonsClicked = 0;
        connectButton.interactable = true;

        // For the 3rd minigame, reset the timer to give the player more time each round
        if (difficultyLevel == 3)
        {
            timeLeft = maxTime;
            timerText.text = "Time: " + timeLeft;
        }

        ClearProtons();
        SpawnProtons(currentBoothNumber);
        StartCoroutine(CountdownTimer());

        // If it's the 3rd minigame, start the movement coroutine
        if (difficultyLevel == 3)
        {
            moveProtonsCoroutine = StartCoroutine(MoveProtonsRandomly());
        }

        currentRound++;
    }



    void ClearProtons()
    {
        foreach (GameObject proton in spawnedProtons)
        {
            if (proton != null)
            {
                Destroy(proton);
            }
        }
        spawnedProtons.Clear();
        Debug.Log("Cleared UI protons.");
    }

    void SpawnProtons(int boothNumber)
    {
        ClearProtons();

        // For minigame 2, use random positions.
        if (boothNumber == 2)
        {
            List<Vector2> usedPositions = new List<Vector2>();
            float minDistance = 100f;
            int maxAttempts = 50;

            // Spawn real protons randomly.
            for (int i = 0; i < 4; i++)
            {
                GameObject proton = Instantiate(protonPrefab, protonContainer);
                spawnedProtons.Add(proton);
                RectTransform rect = proton.GetComponent<RectTransform>();

                Vector2 newPosition;
                int attempts = 0;
                do
                {
                    newPosition = new Vector2(Random.Range(-150, 150), Random.Range(-100, 100));
                    attempts++;
                }
                while (IsOverlapping(newPosition, usedPositions, minDistance) && attempts < maxAttempts);

                usedPositions.Add(newPosition);
                rect.anchoredPosition = newPosition;

                Button protonButton = proton.GetComponent<Button>();
                protonButton.onClick.AddListener(() => OnProtonClicked(proton, protonButton));
            }

            // For booth 2, there are 2 fake protons.
            int fakeProtonCount = 2;
            for (int i = 0; i < fakeProtonCount; i++)
            {
                GameObject fakeProton = Instantiate(fakeProtonPrefab, protonContainer);
                spawnedProtons.Add(fakeProton);
                RectTransform rect = fakeProton.GetComponent<RectTransform>();

                Vector2 newPosition;
                int attempts = 0;
                do
                {
                    newPosition = new Vector2(Random.Range(-150, 150), Random.Range(-100, 100));
                    attempts++;
                }
                while (IsOverlapping(newPosition, usedPositions, minDistance) && attempts < maxAttempts);

                usedPositions.Add(newPosition);
                rect.anchoredPosition = newPosition;

                // Ensure fake protons are rendered behind real ones.
                fakeProton.transform.SetAsFirstSibling();
            }
        }
        else // For other minigames, use a circular (spread out) layout.
        {
            int realProtonCount = 4;
            int fakeProtonCount = 0;
            if (boothNumber >= 2)
            {
                fakeProtonCount = (boothNumber == 2) ? 2 : 4;
            }
            int totalProtons = realProtonCount + fakeProtonCount;
            float radius = 100f;

            for (int i = 0; i < totalProtons; i++)
            {
                float angle = (360f / totalProtons) * i * Mathf.Deg2Rad;
                Vector2 position = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);

                if (i < realProtonCount)
                {
                    GameObject proton = Instantiate(protonPrefab, protonContainer);
                    proton.GetComponent<RectTransform>().anchoredPosition = position;
                    Button protonButton = proton.GetComponent<Button>();
                    protonButton.onClick.AddListener(() => OnProtonClicked(proton, protonButton));
                    spawnedProtons.Add(proton);
                }
                else
                {
                    GameObject fakeProton = Instantiate(fakeProtonPrefab, protonContainer);
                    fakeProton.GetComponent<RectTransform>().anchoredPosition = position;
                    spawnedProtons.Add(fakeProton);
                    fakeProton.transform.SetAsFirstSibling();
                }
            }
        }
    }




    IEnumerator CountdownTimer()
    {
        if (isTimerRunning) yield break;
        isTimerRunning = true;

        while (timeLeft > 0 && isMinigameActive)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            timerText.text = "Time: " + timeLeft;
        }

        isTimerRunning = false;

        if (timeLeft == 0 && isMinigameActive)
        {
            LoseMinigame();
        }
    }

    IEnumerator MoveProtonsRandomly()
    {
        float moveInterval;
        // For rounds 2 and 3 in the 3rd minigame, force normal speed (1 second)
        if (difficultyLevel == 3 && currentRound > 1)
        {
            moveInterval = 1f;
        }
        else
        {
            // Otherwise, use the speed setting from the inspector
            moveInterval = booth3Speed == SpeedLevel.Slow ? 1.5f :
                           (booth3Speed == SpeedLevel.Normal ? 1f : 0.5f);
        }

        while (isMinigameActive)
        {
            foreach (var proton in spawnedProtons)
            {
                if (proton == null) continue;
                RectTransform rect = proton.GetComponent<RectTransform>();
                if (rect == null) continue;

                Vector2 newPosition = new Vector2(Random.Range(-150, 150), Random.Range(-100, 100));
                rect.anchoredPosition = newPosition;
            }
            yield return new WaitForSeconds(moveInterval);
        }
    }


    void OnProtonClicked(GameObject proton, Button button)
    {
        button.interactable = false;
        proton.GetComponent<Image>().color = Color.green;
        protonsClicked++;
    }

    public void OnConnectButtonClicked()
    {
        if (protonsClicked == 4)
        {
            Debug.Log("Connect button clicked. Protons clicked: " + protonsClicked);
            totalProtonsCollected += 4;
            protonCounterText.text = $"Protons: {totalProtonsCollected}/24";

            // Check if more rounds are available.
            if (currentRound < maxRounds)
            {
                StartRound();
            }
            else
            {
                EndMinigame();
            }
        }
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
        StopAllCoroutines();
        moveProtonsCoroutine = null; // Reset our reference
        isTimerRunning = false;
        minigamePanel.SetActive(false);
        foreach (var proton in protonsInBooth)
        {
            if (proton != null)
            {
                Debug.Log("Resuming proton movement: " + proton.gameObject.name);
                proton.ResumeMovement();
            }
        }
    }

    void LoseMinigame()
    {
        StopAllCoroutines();
        isTimerRunning = false;
        timeLeft = 0;
        timerText.text = "Time: 0";

        playerHP--;
        hpText.text = "HP: " + playerHP;

        if (playerHP <= 0)
        {
            GameOver();
        }
        else
        {
            // Reset clicked count and re-enable the connect button if needed
            protonsClicked = 0;
            connectButton.interactable = true;

            SpawnProtons(currentBoothNumber);
            timeLeft = maxTime;
            StartCoroutine(CountdownTimer());

            // Restart moving protons if on difficulty level 3
            if (difficultyLevel == 3)
            {
                StartCoroutine(MoveProtonsRandomly());
            }
        }
    }



    void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    bool IsOverlapping(Vector2 newPos, List<Vector2> existingPositions, float minDist)
    {
        foreach (var pos in existingPositions)
        {
            if (Vector2.Distance(newPos, pos) < minDist)
            {
                return true;
            }
        }
        return false;
    }

    public int TotalProtonsCollected
    {
        get { return totalProtonsCollected; }
    }

}
