using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Tooltip("Reference to your MinigameManager script in the scene")]
    public MinigameManager minigameManager;

    [Tooltip("The winning screen panel to display when the win condition is met")]
    public GameObject winningScreen;

    [Tooltip("Tag of the object that should trigger the win condition (e.g., 'Player')")]
    public string triggeringTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggeringTag))
        {
            // Access the public property instead of the private field.
            if (minigameManager.TotalProtonsCollected >= 24)
            {
                Debug.Log("Win condition met! Displaying winning screen.");
                winningScreen.SetActive(true);
            }
            else
            {
                Debug.Log("Win condition not met. Total protons: " + minigameManager.TotalProtonsCollected);
            }
        }
    }
}
