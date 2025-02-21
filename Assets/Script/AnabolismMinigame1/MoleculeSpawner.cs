using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class MoleculeSpawner : MonoBehaviour
{
    public GameObject[] moleculePrefabs;
    //public float minSpawnRate;
    //public float maxSpawnRate;
    //public float spawnRate;   // Time between each spawn
    //public float difficultyIncreaseRate;

    public float spawnRangeX; // Width of the spawn area
    public float spawnRangeY; // Height of the spawn area

    private TaskManager taskManager;
    public AnabolismGameManager gameManager;

    public bool isGameFinished = false;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>();
        StartCoroutine(SpawnMolecules());
    }

    private void OnDrawGizmos()
    {
        Debug.Log("Drawing Gizmos...");
        Gizmos.color = Color.red;

        Vector3 topLeft = new Vector3(-spawnRangeX, spawnRangeY, 0);
        Vector3 topRight = new Vector3(spawnRangeX, spawnRangeY, 0);
        Vector3 bottomLeft = new Vector3(-spawnRangeX, -spawnRangeY, 0);
        Vector3 bottomRight = new Vector3(spawnRangeX, -spawnRangeY, 0);

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
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                Instantiate(randomMolecule, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(1f);
            //if (taskManager.currentTaskIndex >= taskManager.taskColors.Length)
            //if 
            //{
            //    // All tasks are completed
            //    taskManager.gameOver();
            //    isGameFinished = true;
            //    //spawnRate = minSpawnRate;
            //    Debug.Log("All tasks completed. Stopping spawner.");
            //    yield break; // Exit the coroutine
            //}

        }
    }

    //public float SpawnOverTime()
    //{
    //    spawnRate = Mathf.Lerp(minSpawnRate, maxSpawnRate, Time.time * difficultyIncreaseRate);
    //    return spawnRate;
    //}
}

