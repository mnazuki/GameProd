using UnityEngine;
using TMPro;

public class Ticket : MonoBehaviour
{
    public TextMeshProUGUI gateText;
    public TextMeshProUGUI protonText;
    private bool isGiven = false; // Prevent multiple uses

    public void SetTicketInfo(string gate, int protonCount)
    {
        if (gateText != null)
            gateText.text = "Gate: " + gate;

        if (protonText != null)
            protonText.text = "Protons: " + protonCount;
    }

    // This method should exist in Ticket.cs
    public void GiveToPassenger()
    {
        if (!isGiven)
        {
            isGiven = true;
            Debug.Log("Ticket given to passenger!");
            Destroy(gameObject); // Remove ticket
        }
    }
}
