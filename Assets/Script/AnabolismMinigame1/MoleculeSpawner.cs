using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class MoleculeSpawner : MonoBehaviour
{
    public GameObject[] spherePrefabs; // Array to store Red, Blue, and Yellow sphere prefabs
    public float minSpawnRate;
    public float maxSpawnRate;
    public float spawnRate;   // Time between each spawn
    public float difficultyIncreaseRate;

    public float spawnRangeX; // Width of the spawn area
    public float spawnRangeY; // Height of the spawn area

    private TaskManager taskManager;

    public bool isGameFinished = false;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>();
        StartCoroutine(SpawnSpheres());
    }

    IEnumerator SpawnSpheres()
    {
        while (true)
        {
            if (!isGameFinished)
            {
                GameObject randomSphere = spherePrefabs[Random.Range(0, spherePrefabs.Length)];
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                Instantiate(randomSphere, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(SpawnOverTime());
            }
            if (taskManager.currentTaskIndex >= taskManager.taskColors.Length)
            {
                // All tasks are completed
                taskManager.gameOver();
                isGameFinished = true;
                spawnRate = minSpawnRate;
                Debug.Log("All tasks completed. Stopping spawner.");
                yield break; // Exit the coroutine
            }

        }
    }

    public float SpawnOverTime()
    {
        spawnRate = Mathf.Lerp(minSpawnRate, maxSpawnRate, Time.time * difficultyIncreaseRate);
        return spawnRate;
    }
}

