using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainState { Idle, Moving, WaitingPickup, WaitingDropOff }

public class TrainMovement : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Transform> waypoints;         // Train route waypoints.
    public float speed = 5f;                  // Movement speed.
    public float rotationSpeed = 2f;          // Rotation speed for smooth turning.
    public bool waitForProtonsBeforeStart = false; // If true, train starts idle.
    public float stopDelay = 1f;              // Delay at pickup/drop-off stops.
    public float waypointThreshold = 0.05f;   // How close before considering a waypoint "reached".

    private int currentWaypointIndex = 0;
    private TrainState trainState;
    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;  // For SmoothDamp.
    private bool isWaitingAtStop = false;     // Flag to hold train at a stop.

    [Header("Proton Handling")]
    public List<ProtonMovement> protonsOnBoard = new List<ProtonMovement>();

    public bool IsMoving { get { return trainState == TrainState.Moving; } }
    public TrainState CurrentState { get { return trainState; } }

    [Header("Audio")]
    public AudioSource src; 
    public AudioClip horn;

    void Start()
    {
        if (waitForProtonsBeforeStart)
        {
            trainState = TrainState.Idle;
            Debug.Log($"{gameObject.name} is idle, waiting for protons to board.");
        }
        else
        {
            trainState = TrainState.Moving;
            isMoving = true;
            src.PlayOneShot(horn);
        }
    }

    void Update()
    {
        // Only move if not currently waiting at a stop.
        if (!isWaitingAtStop && trainState == TrainState.Moving && isMoving && currentWaypointIndex < waypoints.Count)
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Smoothly move toward the target.
        float smoothTime = 0.3f;
        transform.position = Vector3.SmoothDamp(transform.position, targetWaypoint.position, ref velocity, smoothTime, speed);

        // Smooth rotation.
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // If we're close enough, snap to the waypoint.
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            transform.position = targetWaypoint.position;
            velocity = Vector3.zero;
            Debug.Log($"{gameObject.name} reached waypoint {currentWaypointIndex}");

            if (targetWaypoint.CompareTag("Pickup"))
            {
                trainState = TrainState.WaitingPickup;
                isMoving = false;
                isWaitingAtStop = true;
                Debug.Log($"{gameObject.name} reached pickup stop. Waiting for protons...");
                StartCoroutine(WaitForPickupAndResume());
            }
            else if (targetWaypoint.CompareTag("DropOff"))
            {
                trainState = TrainState.WaitingDropOff;
                isMoving = false;
                isWaitingAtStop = true;
                Debug.Log($"{gameObject.name} reached drop-off stop. Releasing protons...");
                StartCoroutine(WaitForDropOffAndResume());
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }

    IEnumerator WaitForPickupAndResume()
    {
        // Wait until at least one proton is onboard.
        while (protonsOnBoard.Count == 0)
            yield return null;
        yield return new WaitForSeconds(stopDelay);
        Debug.Log($"{gameObject.name} resuming from pickup stop.");
        trainState = TrainState.Moving;
        isMoving = true;
        // Increment the waypoint index once.
        currentWaypointIndex++;
        isWaitingAtStop = false;
    }

    IEnumerator WaitForDropOffAndResume()
    {
        yield return new WaitForSeconds(stopDelay);
        ReleaseProtons();
        Debug.Log($"{gameObject.name} resuming from drop-off stop.");
        trainState = TrainState.Moving;
        isMoving = true;
        currentWaypointIndex++;
        isWaitingAtStop = false;
    }

    public void OnProtonBoard(ProtonMovement proton)
    {
        if (!protonsOnBoard.Contains(proton))
        {
            protonsOnBoard.Add(proton);
            proton.transform.SetParent(transform);
            proton.SetCarried(true);
            Debug.Log($"{proton.gameObject.name} boarded {gameObject.name}.");
        }
    }

    public void StartTrain()
    {
        // Force the train to start moving.
        isMoving = true;
        trainState = TrainState.Moving;
        Debug.Log($"{gameObject.name} started moving.");
        src.PlayOneShot(horn);
    }

    void ReleaseProtons()
    {
        foreach (ProtonMovement proton in protonsOnBoard)
        {
            if (proton != null)
            {
                proton.transform.SetParent(null);
                proton.SetCarried(false);
                proton.ResumeMovement();
            }
        }
        protonsOnBoard.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ProtonMovement proton = other.GetComponent<ProtonMovement>();
        if (proton != null && !proton.IgnoreTriggers)
        {
            OnProtonBoard(proton);
            if (trainState == TrainState.Idle)
            {
                Debug.Log($"{gameObject.name} was idle; starting movement as a proton boarded.");
                StartTrain();
            }
        }
    }
}
