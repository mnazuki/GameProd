using UnityEngine;
using TMPro; // Import TextMeshPro

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab; // Assign this in Inspector
    public Transform spawnPoint; // Assign this in Inspector
    public TextMeshProUGUI monitorText; // Assign the monitor UI in Inspector

    private string[] gates = { "NADH", "FADH" };

    void Start()
    {
        SpawnPassenger(); // Auto-spawn when the game starts
    }

    public void SpawnPassenger()
    {
        if (passengerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("PassengerPrefab or SpawnPoint is missing!");
            return;
        }

        GameObject newPassenger = Instantiate(passengerPrefab, spawnPoint.position, Quaternion.identity);
        Passenger passengerScript = newPassenger.GetComponent<Passenger>();

        if (passengerScript == null)
        {
            Debug.LogError("Passenger component missing on prefab!");
            return;
        }

        // Assign random values for passenger attributes
        passengerScript.gateType = gates[Random.Range(0, gates.Length)];
        passengerScript.protonCount = Random.Range(1, 5);

        Debug.Log($"Passenger Spawned: Gate = {passengerScript.gateType}, Protons = {passengerScript.protonCount}");

        // **Update Monitor Display**
        UpdateMonitor(passengerScript.gateType, passengerScript.protonCount);
    }

    private void UpdateMonitor(string gate, int protons)
    {
        if (monitorText != null)
        {
            monitorText.text = $"Gate: {gate}\nProtons: {protons}";
        }
        else
        {
            Debug.LogError("Monitor TextMeshProUGUI is not assigned in the Inspector!");
        }
    }
}
