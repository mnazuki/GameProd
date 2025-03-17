using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class MoleculeSpawner : MonoBehaviour
{
    public GameObject[] moleculePrefabs;

    public float spawnRangeX; // Width of the spawn area
    public float spawnRangeY; // Height of the spawn area

    private TaskManager taskManager;
    private CollectionScript collectionScript;
    public AnabolismGameManager gameManager;

    public bool isGameFinished = false;
    [SerializeField] private float spawnDelay = 1.5f;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>();
        StartCoroutine(SpawnMolecules());
    }

    public void UpdateSpawnDelay(int gameRound)
    {
        if (gameRound == 0) spawnDelay = 1.5f;
        else if (gameRound == 1) spawnDelay = 1.0f;
        else if (gameRound == 2) spawnDelay = 0.75f;
        Debug.Log("Spawn delay updated: " + spawnDelay);
    }

    public void RestartSpawning()
    {
        StopAllCoroutines();  // Stop any existing spawning coroutine
        isGameFinished = false;  // Ensure the game is still running
        StartCoroutine(SpawnMolecules());  // Restart spawning
    }


    private void OnDrawGizmos()
    {
        Debug.Log("Drawing Gizmos...");
        Gizmos.color = Color.red;

        float minY = -spawnRangeY * 0.5f; // Adjust the bottom Y limit
        float maxY = spawnRangeY;         // Keep the top limit the same

        Vector3 topLeft = new Vector3(-spawnRangeX, maxY, 0);
        Vector3 topRight = new Vector3(spawnRangeX, maxY, 0);
        Vector3 bottomLeft = new Vector3(-spawnRangeX, minY, 0);
        Vector3 bottomRight = new Vector3(spawnRangeX, minY, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    IEnumerator SpawnMolecules()
    {
        while (!isGameFinished)
        {
                // Pause spawning if hint screen is open
                while (gameManager.hintScreenOpen)
                {
                    yield return null;  // Wait until hint screen closes
                }

                GameObject randomMolecule = moleculePrefabs[Random.Range(0, moleculePrefabs.Length)];
                float minY = -spawnRangeY * 0.5f;
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(minY, spawnRangeY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                Instantiate(randomMolecule, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(spawnDelay);

        }
    }

}

