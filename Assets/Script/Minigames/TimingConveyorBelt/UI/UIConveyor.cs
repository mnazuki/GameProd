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
            winPlane.SetActive(true);
            Time.timeScale = 0.01f;
        }

//gameOverRetryButton.onClick.AddListener(ResetButton);
//        resetButton.onClick.AddListener(ResetButton);
  //      nextSceneButton.onClick.AddListener(NextMinigameButton);
    }

    public void ResetButton()
    {
        Debug.Log("is Reset");

        playerHealthSC.score = 0;

        playerHealthSC.health = playerHealthSC.numberOfHearts;
        playerHealthSC.UpdateHeartsUI();
        playerHealthSC.isGameOver = false;

        gameOverPlane.SetActive(false);
        winPlane.SetActive(false);

        Time.timeScale = 1;

    }
    
    public void NextMinigameButton()
    {
        //SceneManager.LoadScene("Scene Name/Index Num")
        //Put the next scene here ^^^
    }
}
