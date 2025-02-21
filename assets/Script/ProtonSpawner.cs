using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonSpawner : MonoBehaviour
{
    public GameObject protonPrefab;
    public Transform spawnPoint;
    public List<Transform> waypoints; // Assign these in Inspector

    void Start()
    {
        SpawnProton();
    }

    void SpawnProton()
    {
        GameObject newProton = Instantiate(protonPrefab, spawnPoint.position, Quaternion.identity);
        ProtonMovement protonMovement = newProton.GetComponent<ProtonMovement>();

        if (protonMovement != null)
        {
            protonMovement.AssignWaypoints(waypoints); // Assign waypoints correctly
        }
        else
        {
            Debug.LogError("ProtonMovement component not found on spawned proton!");
        }
    }

}
