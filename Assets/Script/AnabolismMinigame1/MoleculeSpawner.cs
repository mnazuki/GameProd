using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class MoleculeSpawner : MonoBehaviour
{
    public GameObject[] moleculePrefabs;

    public float spawnRangeX;
    public float spawnRangeY; 

    [SerializeField] private TaskManager taskManager;
    [SerializeField] private CollectionScript collectionScript;
    public AnabolismGameManager gameManager;

    public bool isGameFinished = false;
    [SerializeField] private float spawnDelay = 1.5f;

    public int maxMolecules = 20;
    [SerializeField] private int currentMoleculeCount = 0;

    public static MoleculeSpawner Instance;

    private void Start()
    {
        Instance = this;
        taskManager = FindAnyObjectByType<TaskManager>();
        collectionScript = FindAnyObjectByType<CollectionScript>();
        StartCoroutine(SpawnMolecules());
        collectionScript.gameRound = 0;
        currentMoleculeCount = 0;
    }

    public void UpdateSpawnDelay(int gameRound)
    {
        if (gameRound == 0) spawnDelay = 1.5f;
        Debug.Log("Spawn delay updated: " + spawnDelay);
    }

    public void RestartSpawning()
    {
        StopAllCoroutines();
        isGameFinished = false;

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SpawnMolecules());
        }
        else
        {
            Debug.LogWarning("Cannot start coroutine because MoleculeSpawner is not active.");
        }
    }


    private void OnDrawGizmos()
    {
        Debug.Log("Drawing Gizmos...");
        Gizmos.color = Color.red;

        float minY = -spawnRangeY * 0.5f; // adjusts bottom y limit
        float maxY = spawnRangeY;

        Vector3 topLeft = new Vector3(-spawnRangeX, maxY, 0);
        Vector3 topRight = new Vector3(spawnRangeX, maxY, 0);
        Vector3 bottomLeft = new Vector3(-spawnRangeX, minY, 0);
        Vector3 bottomRight = new Vector3(spawnRangeX, minY, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    public IEnumerator SpawnMolecules()
    {
        while (!isGameFinished)
        {
            while (gameManager.hintScreenOpen)
            {
                yield return null;
            }

            if (currentMoleculeCount < maxMolecules)
            {
                GameObject randomMolecule = moleculePrefabs[Random.Range(0, moleculePrefabs.Length)];
                float minY = -spawnRangeY * 0.5f;
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(minY, spawnRangeY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                GameObject spawned = Instantiate(randomMolecule, spawnPosition, Quaternion.identity);
                currentMoleculeCount++;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void DecreaseMoleculeCount()
    {
        currentMoleculeCount = Mathf.Max(0, currentMoleculeCount - 1);
    }

}

