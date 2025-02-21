using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoothManager : MonoBehaviour
{
    private List<ProtonMovement> protonsInBooth = new List<ProtonMovement>();
    public MinigameManager minigameManager;
    public int requiredProtons = 2; // Changeable in Inspector
    public int boothNumber; // Make sure this is set in the Inspector

    void Start()
    {
        minigameManager = FindFirstObjectByType<MinigameManager>();
    }

    public void OnProtonEnter(ProtonMovement proton)
    {
        if (!protonsInBooth.Contains(proton))
        {
            protonsInBooth.Add(proton);
        }

        // Only start the minigame when 2 protons are inside the booth
        if (protonsInBooth.Count >= requiredProtons)
        {
            Debug.Log($"Starting Minigame at Booth {boothNumber}");
            List<ProtonMovement> protonsToPass = new List<ProtonMovement>(protonsInBooth);
            minigameManager.StartMinigame(protonsToPass, boothNumber);
        }
    }

    public void OnProtonExit(ProtonMovement proton)
    {
        if (protonsInBooth.Contains(proton))
        {
            protonsInBooth.Remove(proton);
        }
    }

    public Vector3 GetProtonPosition()
    {
        return transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ProtonMovement proton = other.GetComponent<ProtonMovement>();
        if (proton != null && !protonsInBooth.Contains(proton))
        {
            protonsInBooth.Add(proton);
            Debug.Log($"Proton entered booth {boothNumber}: {proton.gameObject.name}");
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        ProtonMovement proton = other.GetComponent<ProtonMovement>();

        if (proton != null)
        {
            Debug.Log($"Proton {proton.gameObject.name} exited Booth {boothNumber}");
            OnProtonExit(proton);
        }
    }

    public void ReleaseProtons()
    {
        Debug.Log("Releasing protons from Booth: " + boothNumber);

        // Create a copy of the list before modifying it
        List<ProtonMovement> protonsToMove = new List<ProtonMovement>(protonsInBooth);

        foreach (var proton in protonsToMove)
        {
            if (proton != null)
            {
                proton.ResumeMovement();
            }
        }

        protonsInBooth.Clear(); // Now clear after iteration
    }



}
