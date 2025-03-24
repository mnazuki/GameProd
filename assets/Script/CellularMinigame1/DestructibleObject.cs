// placed on the NADH, ATP, and Bacteria prefabs (add component)

using UnityEngine;
using System.Collections;

public class DestructibleObject : MonoBehaviour
{
    private HealthManager healthManager;
    Animator animator;
    


    private void Start()
    {
        healthManager = Object.FindFirstObjectByType<HealthManager>(); // finds the HealthManager in the scene
        animator = GetComponent<Animator>();
        
    }

    private void OnMouseDown()
    {
        if (CompareTag("Bacteria")) // check if the clicked object is tagged as "Bacteria"
        {
            if (healthManager != null)
            {
                healthManager.DecreaseHealth();
            }
        }
        
        // collect ATP and NADH if tagged ATP or NADH
        if (CompareTag("ATP") || CompareTag("NADH"))
        {
            // call the spawner to collect the molecule
            // check if the instance is not null, collect the molecule
            if (PyruvateSpawner.Instance != null)
            {
                PyruvateSpawner.Instance.CollectMolecule(gameObject.tag);
            }
        }

        // destroy the object (prefabs where this script is added) when clicked         
            Destroy(gameObject);
    }

 

}
