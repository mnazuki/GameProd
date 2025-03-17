using UnityEngine;

public class CollectionScript : MonoBehaviour
{
    [SerializeField] private int sucroseCollected;
    [SerializeField] private int lactoseCollected;
    [SerializeField] private int maltoseCollected;
    [SerializeField] private int dnaCollected;

    public TaskManager taskManager;
    public MoleculeSpawner moleculeSpawner;

    [SerializeField] private bool gameEnded = false;
    public int gameRound = 0;

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
        if (!gameEnded && sucroseCollected == 2 && lactoseCollected == 2 && maltoseCollected == 2 && dnaCollected == 2)
        {
            gameRound++;
            Debug.Log("Round " + gameRound + " completed!");
            taskManager.nextRound(); // Call TaskManager to start next round
        }
    }

    public void ResetCollection()
    {
        sucroseCollected = 0;
        lactoseCollected = 0;
        maltoseCollected = 0;
        dnaCollected = 0;
        gameEnded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CombinationScript.moleculeInstances.Contains(collision.gameObject))
        {
            Debug.Log("Collected: " + collision.gameObject.name);
            if (collision.gameObject.name.Contains("Sucrose"))
            {
                sucroseCollected++;
                taskManager.AddScore();
                Debug.Log("Sucrose collected. Total: " + sucroseCollected);
                CheckForRoundCompletion();

            }
            else if (collision.gameObject.name.Contains("Lactose"))
            {
                lactoseCollected++;
                taskManager.AddScore();
                Debug.Log("Lactose collected. Total: " + lactoseCollected);
                CheckForRoundCompletion();
            }
            else if (collision.gameObject.name.Contains("Maltose"))
            {
                maltoseCollected++;
                taskManager.AddScore();
                Debug.Log("Maltose collected. Total: " + maltoseCollected);
                CheckForRoundCompletion();
            }
            else if (collision.gameObject.name.Contains("DNA"))
            {
                dnaCollected++;
                taskManager.AddScore();
                Debug.Log("DNA collected. Total: " + dnaCollected);
                CheckForRoundCompletion();
                    
            }
            CombinationScript.moleculeInstances.Remove(collision.gameObject);
            Destroy(collision.gameObject);

        }
        else
        {
            Destroy(collision.gameObject);
            Debug.Log("Not a valid object to collect.");
        }
    }
}
