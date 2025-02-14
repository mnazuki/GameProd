using UnityEngine;

public class PassengerInteraction : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Ticket ticket = other.GetComponent<Ticket>();

        if (ticket != null)
        {
            ticket.GiveToPassenger(); // Remove ticket
            Debug.Log("Ticket given to passenger!");
        }
    }
}
