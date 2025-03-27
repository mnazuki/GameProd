using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassengerUI : MonoBehaviour
{
    public TextMeshProUGUI passengerInfoText;
    public Button giveTicketButton;

    private Passenger currentPassenger;

    void Start()
    {
        giveTicketButton.gameObject.SetActive(true);
        giveTicketButton.onClick.AddListener(GiveTicket);
    }

    public void ShowPassengerInfo(Passenger passenger)
    {
        if (passenger == null)
        {
            Debug.LogError("Passenger data is missing!");
            return;
        }

        currentPassenger = passenger;

        // Debug to check values
        Debug.Log($"Showing UI for: Gate = {passenger.gateType}, Protons = {passenger.protonCount}");

        // Update UI text properly
        passengerInfoText.text = $"Gate: {passenger.gateType}\nProtons: {passenger.protonCount}";
        giveTicketButton.gameObject.SetActive(true);
    }

    private void GiveTicket()
    {
        if (currentPassenger != null)
        {
            currentPassenger.ReceiveTicket();
            giveTicketButton.gameObject.SetActive(false);
        }
    }
}
