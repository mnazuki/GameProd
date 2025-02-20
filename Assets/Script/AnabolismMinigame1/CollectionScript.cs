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

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (sucroseCollected > 0 || lactoseCollected > 0 || maltoseCollected > 0 || dnaCollected > 0)
            {
                taskManager.AddScore();
            }
            CheckForGameCompletion();
        }
    }


    void CheckForGameCompletion()
    {
        if (!gameEnded && sucroseCollected == 2 && lactoseCollected == 2 && maltoseCollected == 2 && dnaCollected == 2)
        {
            gameEnded = true;
            moleculeSpawner.isGameFinished = true;
            taskManager.gameOver();
            Debug.Log("Game completed!");
        }
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
                CheckForGameCompletion();

            }
            else if (collision.gameObject.name.Contains("Lactose"))
            {
                lactoseCollected++;
                taskManager.AddScore();
                Debug.Log("Lactose collected. Total: " + lactoseCollected);
                CheckForGameCompletion();
            }
            else if (collision.gameObject.name.Contains("Maltose"))
            {
                maltoseCollected++;
                taskManager.AddScore();
                Debug.Log("Maltose collected. Total: " + maltoseCollected);
                CheckForGameCompletion();
            }
            else if (collision.gameObject.name.Contains("DNA"))
            {
                dnaCollected++;
                taskManager.AddScore();
                Debug.Log("DNA collected. Total: " + dnaCollected);
                CheckForGameCompletion();

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
