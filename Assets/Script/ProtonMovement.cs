using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonMovement : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>(); // Waypoints assigned by ProtonSpawner
    public float speed = 2f; // Movement speed
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private BoothManager currentBooth;

    // Assigns waypoints from ProtonSpawner
    public void AssignWaypoints(List<Transform> assignedWaypoints)
    {
        waypoints = assignedWaypoints;
        currentWaypointIndex = 0; // Start at the first waypoint
        isMoving = true; // Ensure proton starts moving
    }

    void Update()
    {
        if (isMoving && currentWaypointIndex < waypoints.Count)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Count) return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            if (waypoints[currentWaypointIndex].CompareTag("Booth"))
            {
                BoothManager booth = waypoints[currentWaypointIndex].GetComponent<BoothManager>();
                if (booth != null)
                {
                    StopAtBooth(booth);
                }
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }

    void StopAtBooth(BoothManager booth)
    {
        isMoving = false;
        currentBooth = booth;
        booth.OnProtonEnter(this); // Notify BoothManager
    }

    public void ResumeMovement()
    {
        if (currentBooth != null)
        {
            currentBooth.OnProtonExit(this);
            currentBooth = null; // Reset booth reference
        }

        isMoving = true;
        currentWaypointIndex++; // Move to the next waypoint
        Debug.Log($"{gameObject.name} is resuming movement.");
    }

}
