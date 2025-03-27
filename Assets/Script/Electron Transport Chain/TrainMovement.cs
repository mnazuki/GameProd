using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainState { Idle, Moving, WaitingPickup, WaitingDropOff }

public class TrainMovement : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Transform> waypoints;    // Train route waypoints
    public float speed = 5f;             // Movement speed
    public float rotationSpeed = 2f;     // How quickly the train rotates
    public bool waitForProtonsBeforeStart = false; // If true, starts idle until a proton boards
    public float stopDelay = 1f;         // Delay at pickup/drop-off stops

    private int currentWaypointIndex = 0;
    private TrainState trainState;
    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;  // For SmoothDamp

    [Header("Proton Handling")]
    public List<ProtonMovement> protonsOnBoard = new List<ProtonMovement>();

    public bool IsMoving { get { return trainState == TrainState.Moving; } }
    public TrainState CurrentState { get { return trainState; } }

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
        }
    }

    void Update()
    {
        if (trainState == TrainState.Moving && isMoving && currentWaypointIndex < waypoints.Count)
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Smooth movement using SmoothDamp
        float smoothTime = 0.3f;
        transform.position = Vector3.SmoothDamp(transform.position, targetWaypoint.position, ref velocity, smoothTime, speed);

        // Smoothly rotate toward the waypoint.
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (targetWaypoint.CompareTag("Pickup"))
            {
                trainState = TrainState.WaitingPickup;
                isMoving = false;
                Debug.Log($"{gameObject.name} reached pickup stop. Waiting for protons...");
                StartCoroutine(WaitForPickupAndResume());
            }
            else if (targetWaypoint.CompareTag("DropOff"))
            {
                trainState = TrainState.WaitingDropOff;
                isMoving = false;
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
        currentWaypointIndex++; // Move past pickup stop.
    }

    IEnumerator WaitForDropOffAndResume()
    {
        yield return new WaitForSeconds(stopDelay);
        ReleaseProtons();
        Debug.Log($"{gameObject.name} resuming from drop-off stop.");
        trainState = TrainState.Moving;
        isMoving = true;
        currentWaypointIndex++; // Move past drop-off stop.
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
        if (!isMoving)
        {
            isMoving = true;
            trainState = TrainState.Moving;
            Debug.Log($"{gameObject.name} started moving.");
        }
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
        if (proton != null)
        {
            OnProtonBoard(proton);
            if (trainState == TrainState.Idle)
            {
                Debug.Log($"{gameObject.name} was idle; starting movement as a proton boarded.");
                trainState = TrainState.Moving;
                isMoving = true;
            }
        }
    }
}
