using UnityEngine;

public class Passenger : MonoBehaviour
{
    public string gateType; // Example: "NADH", "FADH₂"
    public int protonCount;

    private bool hasTicket = false;

    public void ReceiveTicket()
    {
        if (!hasTicket)
        {
            hasTicket = true;
            Debug.Log($"Passenger received ticket: {gateType} - {protonCount}");

            // Spawn a new passenger before destroying the current one
            FindObjectOfType<PassengerSpawner>().SpawnPassenger();

            // Destroy the current passenger
            Destroy(gameObject);
        }
    }
}
