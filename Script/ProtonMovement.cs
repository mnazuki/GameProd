using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonMovement : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>(); // Assigned via ProtonSpawner
    public float speed = 2f;         // Movement speed
    public float smoothTime = 0.2f;    // Smoothing time for SmoothDamp
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private bool isCarried = false;    // True when proton is carried by a train
    private BoothManager currentBooth;
    private Vector3 velocity = Vector3.zero; // For SmoothDamp

    // Flag to temporarily ignore triggers after release.
    private bool ignoreTriggers = false;
    public bool IgnoreTriggers { get { return ignoreTriggers; } }

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
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Count)
            return;

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime, speed);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target.CompareTag("Booth"))
            {
                BoothManager booth = target.GetComponent<BoothManager>();
                if (booth != null)
                {
                    StopAtBooth(booth);
                    return;
                }
            }
            else if (target.CompareTag("Train"))
            {
                TrainMovement train = target.GetComponent<TrainMovement>();
                if (train != null)
                {
                    StopAtTrain(train);
                    return;
                }
            }
            else if (target.CompareTag("Exit"))
            {
                StopAtExit();
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
        train.StartTrain();
    }

    public void StopAtExit()
    {
        isMoving = false;
        Debug.Log($"{gameObject.name} has reached the exit and stopped.");
        MinigameManager mgm = Object.FindFirstObjectByType<MinigameManager>();
        if (mgm != null)
            mgm.ProtonReachedExit(this);
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
        currentWaypointIndex++; // Advance to next waypoint.
        ignoreTriggers = true;
        StartCoroutine(ResetIgnoreTriggers());
        Debug.Log($"{gameObject.name} is resuming movement. Parent now: {transform.parent}");
    }

    IEnumerator ResetIgnoreTriggers()
    {
        yield return new WaitForSeconds(1f);
        ignoreTriggers = false;
    }
}
