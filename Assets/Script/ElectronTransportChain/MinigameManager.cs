using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum SpeedLevel { Slow, Normal, Fast }

public class MinigameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject minigamePanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI protonCounterText;
    public GameObject gameOverScreen;    // Lose panel (e.g. d2)
    public GameObject winningScreen;     // Win panel (e.g. d3)
    public Button connectButton;
    public Button resetButton;

    [Header("Dialogue")]
    public GameObject dialoguePanel;     // The dialogue panel that must finish before the game starts

    [Header("Hearts UI")]
    public Image[] heartImages;          // Exactly 3 heart images for 3 HP
    public Sprite fullHeart;             // Full heart sprite
    public Sprite emptyHeart;            // Empty heart sprite

    [Header("Proton Spawning")]
    public GameObject protonPrefab;
    public GameObject fakeProtonPrefab;
    public Transform protonContainer;
    private List<GameObject> spawnedProtons = new List<GameObject>();

    [Header("Game Variables")]
    public int playerHP = 3;             // Logical HP remains 3
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
    private Coroutine moveProtonsCoroutine;

    [Header("Speed Settings for Booth 3")]
    public SpeedLevel booth3Speed = SpeedLevel.Normal;

    // For win condition: track protons that reached the exit.
    private List<ProtonMovement> exitProtons = new List<ProtonMovement>();

    [Header("Dialogue & Audio")]
    public GameObject d2;   // Lose dialogue trigger (when null, lose UI appears)
    public GameObject d3;   // Win dialogue trigger (when null, win UI appears)
    public AudioSource bgmsrc;
    public AudioClip bgm;

    [Header("Star UI Elements")]
    public Image[] starImages;           // Assign star image UI elements in inspector
    public Sprite fullStarSprite;        // Full star sprite
    public Sprite emptyStarSprite;       // Empty star sprite

    void Start()
    {
        // Start background music.
        bgmsrc.loop = true;
        bgmsrc.clip = bgm;
        bgmsrc.Play();

        gameOverScreen.SetActive(false);
        winningScreen.SetActive(false);
        minigamePanel.SetActive(false);
        protonCounterText.text = $"{totalProtonsCollected}/24";
        resetButton.onClick.AddListener(ResetGame);
        UpdateHeartsUI();

        // Wait for the dialogue to finish before starting the game.
        if (dialoguePanel != null)
        {
            StartCoroutine(WaitForDialogueToEnd());
        }
        else
        {
            BeginGame();
        }
    }

    IEnumerator WaitForDialogueToEnd()
    {
        // Wait until the dialogue panel is no longer active.
        while (dialoguePanel.activeSelf)
        {
            yield return null;
        }
        BeginGame();
    }

    void BeginGame()
    {
        // Dialogue is finished; now start the game.
        Debug.Log("Dialogue finished. Starting game...");
        minigamePanel.SetActive(true);
        // You can optionally call additional initialization here.
    }

    void Update()
    {
        // Remove or comment out this block to prevent immediate win UI activation.
        // if (d3 == null)
        // {
        //     bgmsrc.Stop();
        //     winningScreen.SetActive(true);
        //     Debug.Log("Win UI is now active.");
        // }
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

        if (moveProtonsCoroutine != null)
        {
            StopCoroutine(moveProtonsCoroutine);
            moveProtonsCoroutine = null;
        }

        protonsClicked = 0;
        connectButton.interactable = true;

        if (difficultyLevel == 3)
        {
            timeLeft = maxTime;
            timerText.text = "Time: " + timeLeft;
        }

        ClearProtons();
        SpawnProtons(currentBoothNumber);
        StartCoroutine(CountdownTimer());

        if (difficultyLevel == 3)
            moveProtonsCoroutine = StartCoroutine(MoveProtonsRandomly());

        currentRound++;
    }

    void ClearProtons()
    {
        foreach (GameObject proton in spawnedProtons)
        {
            if (proton != null)
                Destroy(proton);
        }
        spawnedProtons.Clear();
        Debug.Log("Cleared UI protons.");
    }

    void SpawnProtons(int boothNumber)
    {
        ClearProtons();
        if (boothNumber == 2)
        {
            List<Vector2> usedPositions = new List<Vector2>();
            float minDistance = 100f;
            int maxAttempts = 50;
            for (int i = 0; i < 4; i++)
            {
                GameObject proton = Instantiate(protonPrefab, protonContainer);
                spawnedProtons.Add(proton);
                RectTransform rect = proton.GetComponent<RectTransform>();
                Vector2 newPosition;
                int attempts = 0;
                do
                {
                    newPosition = new Vector2(UnityEngine.Random.Range(-150, 150), UnityEngine.Random.Range(-100, 100));
                    attempts++;
                }
                while (IsOverlapping(newPosition, usedPositions, minDistance) && attempts < maxAttempts);
                usedPositions.Add(newPosition);
                rect.anchoredPosition = newPosition;
                Button protonButton = proton.GetComponent<Button>();
                protonButton.onClick.AddListener(() => OnProtonClicked(proton, protonButton));
            }
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
                    newPosition = new Vector2(UnityEngine.Random.Range(-150, 150), UnityEngine.Random.Range(-100, 100));
                    attempts++;
                }
                while (IsOverlapping(newPosition, usedPositions, minDistance) && attempts < maxAttempts);
                usedPositions.Add(newPosition);
                rect.anchoredPosition = newPosition;
                fakeProton.transform.SetAsFirstSibling();
            }
        }
        else
        {
            int realProtonCount = 4;
            int fakeProtonCount = (boothNumber == 3) ? 4 : 0;
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
            LoseMinigame();
    }

    IEnumerator MoveProtonsRandomly()
    {
        float moveInterval;
        if (booth3Speed == SpeedLevel.Slow)
            moveInterval = 1.5f;
        else if (booth3Speed == SpeedLevel.Normal)
            moveInterval = 1f;
        else
            moveInterval = 0.5f;
        if (difficultyLevel == 3 && currentRound > 1)
            moveInterval = 1f;
        while (isMinigameActive)
        {
            foreach (var proton in spawnedProtons)
            {
                if (proton == null) continue;
                RectTransform rect = proton.GetComponent<RectTransform>();
                if (rect == null) continue;
                Vector2 newPosition = new Vector2(UnityEngine.Random.Range(-150, 150), UnityEngine.Random.Range(-100, 100));
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
            protonCounterText.text = $"{totalProtonsCollected}/24";
            if (currentRound < maxRounds)
                StartRound();
            else
                EndMinigame();
        }
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
        StopAllCoroutines();
        moveProtonsCoroutine = null;
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
        UpdateHeartsUI();
        if (playerHP <= 0)
            GameOver();
        else
        {
            protonsClicked = 0;
            connectButton.interactable = true;
            SpawnProtons(currentBoothNumber);
            timeLeft = maxTime;
            StartCoroutine(CountdownTimer());
            if (difficultyLevel == 3)
                StartCoroutine(MoveProtonsRandomly());
        }
    }

    void GameOver()
    {
        StartCoroutine(WaitForLoseDialogueAndShowGameOverScreen());
    }

    private IEnumerator WaitForLoseDialogueAndShowGameOverScreen()
    {
        // Activate the lose dialogue if it's not active
        if (d2 != null && !d2.activeSelf)
        {
            d2.SetActive(true);
        }

        // Wait until the lose dialogue (d2) is no longer active (i.e. finished)
        while (d2 != null && d2.activeSelf)
        {
            yield return null;
        }

        // Optionally wait a short moment after dialogue ends
        yield return new WaitForSeconds(0.5f);

        bgmsrc.Stop();
        gameOverScreen.SetActive(true);
        Debug.Log("Lose UI is now active.");
    }

    public void WinGame()
    {
        if (GetExitProtonCount() >= 2 && totalProtonsCollected >= 24)
        {
            StartCoroutine(WaitForWinDialogueAndShowWinScreen());
        }
    }

    private IEnumerator WaitForWinDialogueAndShowWinScreen()
    {
        // Activate the win dialogue if it's not already active.
        if (d3 != null && !d3.activeSelf)
        {
            d3.SetActive(true);
        }

        // Wait until the win dialogue (d3) is no longer active.
        while (d3 != null && d3.activeSelf)
        {
            yield return null;
        }

        // Optional: wait a moment after dialogue ends.
        yield return new WaitForSeconds(0.5f);

        // Now stop background music and show the win screen.
        bgmsrc.Stop();
        winningScreen.SetActive(true);
        Debug.Log("Win UI is now active.");

        // Animate the stars.
        Star();
    }

    public void ProtonReachedExit(ProtonMovement proton)
    {
        if (!exitProtons.Contains(proton))
            exitProtons.Add(proton);
        if (exitProtons.Count >= 2 && totalProtonsCollected >= 24)
        {
            WinGame();
        }
    }

    int GetExitProtonCount()
    {
        return exitProtons.Count;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetMinigame()
    {
        // Hide lose and win UI elements
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (winningScreen != null) winningScreen.SetActive(false);
        if (d2 != null) d2.SetActive(false);
        if (d3 != null) d3.SetActive(false);

        // Resume game time
        Time.timeScale = 1;

        // Restart background music if it isn't playing
        if (bgmsrc != null && !bgmsrc.isPlaying)
        {
            bgmsrc.Play();
        }

        // Reset player's health to full (assuming full health is 3)
        playerHP = 3;
        UpdateHeartsUI();

        // Stop all running coroutines and reset minigame state
        StopAllCoroutines();
        moveProtonsCoroutine = null;
        isTimerRunning = false;

        // Reset timer and update timer UI
        timeLeft = maxTime;
        timerText.text = "Time: " + timeLeft;
        protonsClicked = 0;
        connectButton.interactable = true;

        // Clear previous protons and re-spawn new ones
        ClearProtons();
        SpawnProtons(currentBoothNumber);

        // Restart countdown timer (and proton movement if needed)
        StartCoroutine(CountdownTimer());
        if (difficultyLevel == 3)
            moveProtonsCoroutine = StartCoroutine(MoveProtonsRandomly());

        isMinigameActive = true;
    }

    bool IsOverlapping(Vector2 newPos, List<Vector2> existingPositions, float minDist)
    {
        foreach (var pos in existingPositions)
        {
            if (Vector2.Distance(newPos, pos) < minDist)
                return true;
        }
        return false;
    }

    public int TotalProtonsCollected { get { return totalProtonsCollected; } }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < playerHP)
                heartImages[i].sprite = fullHeart;
            else
                heartImages[i].sprite = emptyHeart;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit game requested.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Added star function with update to star UI
    public void Star()
    {
        int stars = 0;
        if (totalProtonsCollected >= 24 && exitProtons.Count >= 2)
        {
            stars = 3;
        }
        else if (totalProtonsCollected >= 16)
        {
            stars = 2;
        }
        else if (totalProtonsCollected > 0)
        {
            stars = 1;
        }
        else
        {
            stars = 0;
        }
        Debug.Log("Star rating: " + stars);
        StartCoroutine(AnimateStarUI(stars));
    }
    private IEnumerator AnimateStarUI(int stars)
    {
        // Loop through each star image.
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < stars)
            {
                starImages[i].sprite = fullStarSprite;
            }
            else
            {
                starImages[i].sprite = emptyStarSprite;
            }
            // Delay before updating the next star.
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Updates the star UI images based on the star rating.
    private void UpdateStarUI(int stars)
    {
        if (starImages == null || starImages.Length == 0)
        {
            Debug.LogWarning("Star UI elements are not assigned.");
            return;
        }
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < stars)
            {
                starImages[i].sprite = fullStarSprite;
            }
            else
            {
                starImages[i].sprite = emptyStarSprite;
            }
        }
    }
}
