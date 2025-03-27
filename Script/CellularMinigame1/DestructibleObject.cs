// Placed on the NADH, ATP, and Bacteria prefabs (add component)

using UnityEngine;
using System.Collections;

public class DestructibleObject : MonoBehaviour
{
    private HealthManager healthManager;
    Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip molClick, hurt;
    


    private void Start()
    {
        healthManager = Object.FindFirstObjectByType<HealthManager>(); // finds the HealthManager in the scene
        animator = GetComponent<Animator>();
        src = GameObject.Find("SFX")?.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {

        if (CompareTag("Bacteria")) // check if the clicked object is tagged as "Bacteria"
        {
            src.clip = hurt; src.PlayOneShot(hurt);
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
            src.clip = molClick; src.PlayOneShot(molClick);
            Destroy(gameObject);
    }

 

}
