using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonSpawner : MonoBehaviour
{
    public GameObject protonPrefab;
    public Transform spawnPoint;
    public List<Transform> waypoints; // Set in Inspector

    void Start()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is NOT assigned in ProtonSpawner2!");
            return;
        }
        if (protonPrefab == null)
        {
            Debug.LogError("Proton Prefab is NOT assigned in ProtonSpawner2!");
            return;
        }
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Waypoints list is empty in ProtonSpawner2!");
            return;
        }
        Debug.Log("ProtonSpawner2 Start() running...");
        SpawnProton();
    }

    void SpawnProton()
    {
        GameObject newProton = Instantiate(protonPrefab, spawnPoint.position, Quaternion.identity);
        ProtonMovement protonMovement = newProton.GetComponent<ProtonMovement>();

        if (protonMovement != null)
        {
            protonMovement.AssignWaypoints(waypoints);
        }
        else
        {
            Debug.LogError("ProtonMovement component not found on spawned proton!");
        }
    }
}
