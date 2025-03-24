using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonMovement : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>(); // Set via ProtonSpawner
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private bool isCarried = false;
    private BoothManager currentBooth;

    public void AssignWaypoints(List<Transform> assignedWaypoints)
    {
        waypoints = assignedWaypoints;
        currentWaypointIndex = 0;
        isMoving = true;
        isCarried = false;
    }

    public void SetCarried(bool value)
    {
        isCarried = value;
        isMoving = !value;
    }

    void Update()
    {
        if (!isCarried && isMoving && currentWaypointIndex < waypoints.Count)
            MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Count)
            return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            if (waypoints[currentWaypointIndex].CompareTag("Booth"))
            {
                BoothManager booth = waypoints[currentWaypointIndex].GetComponent<BoothManager>();
                if (booth != null)
                {
                    StopAtBooth(booth);
                    return;
                }
            }
            else if (waypoints[currentWaypointIndex].CompareTag("Train"))
            {
                TrainMovement train = waypoints[currentWaypointIndex].GetComponent<TrainMovement>();
                if (train != null)
                {
                    StopAtTrain(train);
                    return;
                }
            }
            else if (waypoints[currentWaypointIndex].CompareTag("Exit"))
            {
                StopAtExit();
                MinigameManager mgm = Object.FindFirstObjectByType<MinigameManager>();
                if (mgm != null)
                    mgm.WinGame();
                return;
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
        booth.OnProtonEnter(this);
    }

    public void StopAtTrain(TrainMovement train)
    {
        isMoving = false;
        SetCarried(true);
        transform.SetParent(train.transform);
        train.OnProtonBoard(this);
        Debug.Log($"{gameObject.name} has boarded train {train.gameObject.name}");
        if (!train.IsMoving)
            train.StartTrain();
    }

    public void StopAtExit()
    {
        isMoving = false;
        Debug.Log($"{gameObject.name} has reached the exit and stopped.");
    }

    public void ResumeMovement()
    {
        if (currentBooth != null)
        {
            currentBooth.OnProtonExit(this);
            currentBooth = null;
        }
        transform.SetParent(null);
        SetCarried(false);
        isMoving = true;
        currentWaypointIndex++;
        Debug.Log($"{gameObject.name} is resuming movement.");
    }
}
