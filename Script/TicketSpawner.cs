using UnityEngine;
using TMPro; // Required for TextMeshPro

public class TicketSpawner : MonoBehaviour
{
    public GameObject ticketPrefab; // Assign Ticket prefab in Inspector
    public Transform spawnPoint;    // Assign a spawn location in Inspector
    public TextMeshProUGUI monitorText; // Assign the monitor text in Inspector

    private GameObject currentTicket; // Store reference to current ticket

    public void SpawnTicket()
    {
        // Prevent multiple tickets from spawning at once
        if (currentTicket != null)
        {
            Destroy(currentTicket);
        }

        Debug.Log("Monitor Text: " + monitorText.text); // Debug to confirm monitor text

        // Instantiate a new ticket at the spawn position
        currentTicket = Instantiate(ticketPrefab, spawnPoint.position, Quaternion.identity);

        // **Check if the ticket has a Ticket script and pass data**
        Ticket ticketScript = currentTicket.GetComponent<Ticket>();
        if (ticketScript != null && monitorText != null)
        {
            string[] monitorLines = monitorText.text.Split('\n'); // Split into lines
            if (monitorLines.Length >= 2) // Ensure we have gate and proton info
            {
                string gate = monitorLines[0].Replace("Gate: ", "").Trim();
                int protonCount = int.Parse(monitorLines[1].Replace("Protons: ", "").Trim());

                Debug.Log("Assigning to Ticket -> Gate: " + gate + " | Protons: " + protonCount);

                ticketScript.SetTicketInfo(gate, protonCount);
            }
        }
        else
        {
            Debug.LogError("Ticket script or Monitor Text is missing!");
        }
    }
}
