// Placed on Game Manager Spawner at the hierarchy (add component)
// Used by buttons to spawn glucose, form pyruvate, and spawn ATP, NADH, and Bacteria (onclick)

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject glucosePrefab;
    [SerializeField] private GameObject pyruvate1Prefab;
    [SerializeField] private GameObject pyruvate2Prefab;
    [SerializeField] private GameObject nadhPrefab;
    [SerializeField] private GameObject atpPrefab;
    [SerializeField] private GameObject bacteriaPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform glucoseSpawnPoint;
    [SerializeField] private Transform pyruvateSpawnPoint;
    [SerializeField] private Transform ATP_NADHspawnPoint1, ATP_NADHspawnPoint2, ATP_NADHspawnPoint3, ATP_NADHspawnPoint4;
    [SerializeField] private Transform bacteriaSpawnPoint1, bacteriaSpawnPoint2, bacteriaSpawnPoint3, bacteriaSpawnPoint4, bacteriaSpawnPoint5, bacteriaSpawnPoint6;


    [Header("Energy System")]
    [SerializeField] private EnergyBar energyBar; // Energy bar that will be lessened when forming pyruvate
    [SerializeField] private float energyCostToFormPyruvate; // Energy cost to form pyruvate, 2 (edit in inspector)
    public bool emptyEnergy = false; //[NEW] For lose condition

    private GameObject currentGlucose;
    private float pyruvateSeparation = 2f; // distance between pyruvates

    // spawns glucose if no pyruvate exists
    public void SpawnGlucose()
    {
        if (GameObject.FindWithTag("Pyruvate") == null && GameObject.FindWithTag("Glucose") == null && GameObject.FindWithTag("MiniPyruvate") == null)
        {
            //[NEW] Moved this here from "Form Pyruvate" in order for lose condition to occur on Import Glucose click.
            if (!energyBar.CanUseEnergy(energyCostToFormPyruvate))
            {
            emptyEnergy = true; //[NEW] True for lose condition
            }else{
            currentGlucose = Instantiate(glucosePrefab, glucoseSpawnPoint.position, Quaternion.identity);PlaySpawnAnim(currentGlucose);

            // reset ATP & NADH collection count
            if (PyruvateSpawner.Instance != null)
            {
                PyruvateSpawner.Instance.ResetCollection();
            }}
        }
        // [[DEBUG SECTION]] else if (GameObject.FindWithTag("Pyruvate") != null)
        // {
        //     // Script: Warning 
        //     Debug.Log("Cannot spawn glucose while pyruvates exist.");
        // }
        // else if (GameObject.FindWithTag("Glucose") != null)
        // {
        //     // Script: Warning 
        //     Debug.Log("Cannot spawn. Glucose already exists.");
        // }
        // else if (GameObject.FindWithTag("MiniPyruvate") != null)
        // {
        //     // Script: Warning 
        //     Debug.Log("Cannot spawn. Pyruvate is not transported yet / Pyruvate is not ywt oxidized.");
        // }
    }

    
    // converts glucose to pyruvate and removes glucose (only if enough energy is available)
    public void FormPyruvate()
    {
        if (currentGlucose != null && energyBar.CanUseEnergy(energyCostToFormPyruvate))
        {
            // Deduct energy before forming pyruvate
            energyBar.ReduceEnergyBar(energyCostToFormPyruvate);

            // Spawn pyruvates at a set distance apart
            GameObject pyruvate1 = Instantiate(pyruvate1Prefab, pyruvateSpawnPoint.position + new Vector3(-pyruvateSeparation / 2, 0, 0), Quaternion.identity);
            GameObject pyruvate2 = Instantiate(pyruvate2Prefab, pyruvateSpawnPoint.position + new Vector3(pyruvateSeparation / 2, 0, 0), Quaternion.identity);


            // Remove glucose
            PlayExitAnim(currentGlucose);//Exit anim has DestroyObject event on last frame.
            

            // Spawn ATP and NADH at specific spawn points
            SpawnMolecules();

            // Spawn bacteria at specific spawn points
            SpawnBacteria();
        }
        // [[DEBUG SECTION]] else if (currentGlucose == null){
        //     // Script: Warning 
        //     Debug.Log("No glucose to convert to pyruvate!");
        // }

        // if there's not enough energy to for pyruvate to be formed
        
    }

    // Randomly spawns NADH and ATP at assigned spawn points
    private void SpawnMolecules()
    {
        List<Transform> availableSpawnPoints = new List<Transform> { 
            ATP_NADHspawnPoint1, ATP_NADHspawnPoint2, ATP_NADHspawnPoint3, ATP_NADHspawnPoint4
        };

        // shuffle the spawn points list
        System.Random random = new System.Random();
        availableSpawnPoints = availableSpawnPoints.OrderBy(x => random.Next()).ToList();

        // spawn 2 NADH at the first two random positions
        GameObject nadh1 = Instantiate(nadhPrefab, availableSpawnPoints[0].position, Quaternion.identity); PlaySpawnAnim(nadh1);//[NEW] Calls spawn animation from below
        GameObject nadh2 = Instantiate(nadhPrefab, availableSpawnPoints[1].position, Quaternion.identity); PlaySpawnAnim(nadh2);//[NEW] Calls spawn animation from below
       

        // spawn 2 ATP at the next two random positions
        GameObject atp1 = Instantiate(atpPrefab, availableSpawnPoints[2].position, Quaternion.identity); PlaySpawnAnim(atp1);//[NEW] Calls spawn animation from below
        GameObject atp2 = Instantiate(atpPrefab, availableSpawnPoints[3].position, Quaternion.identity); PlaySpawnAnim(atp2);//[NEW] Calls spawn animation from below

        // destroy after 2 seconds, from method DestroyAfterTime
        StartCoroutine(DestroyAfterTime(nadh1, 2f)); 
        StartCoroutine(DestroyAfterTime(nadh2, 2f));
        StartCoroutine(DestroyAfterTime(atp1, 2f));
        StartCoroutine(DestroyAfterTime(atp2, 2f));
    }

    // Randomly spawns 2-4 bacteria at available spawn points
    private void SpawnBacteria()
    {
        // list of available spawn points
        List<Transform> availableSpawnPoints = new List<Transform>{ 
            bacteriaSpawnPoint1, bacteriaSpawnPoint2, bacteriaSpawnPoint3, 
            bacteriaSpawnPoint4, bacteriaSpawnPoint5, bacteriaSpawnPoint6
        };

        // shuffle spawn points, randomize selection
        System.Random random = new System.Random(); // random number generator
        availableSpawnPoints = availableSpawnPoints.OrderBy(x => random.Next()).ToList(); // shuffle spawn points

        // randomly choose how many bacteria to spawn (2-4)
        int bacteriaCount = random.Next(2, 5);

        // spawn bacteria at randomly chosen spawn points
        for (int i = 0; i < bacteriaCount; i++)
        {
            GameObject bacteria = Instantiate(bacteriaPrefab, availableSpawnPoints[i].position, Quaternion.identity); PlaySpawnAnim(bacteria); //[NEW] Calls spawn animation from below
             StartCoroutine(DestroyAfterTime(bacteria, 2f)); // destroy bacteria after a few seconds
        }
    }

    // Destroys object after a set time
    private IEnumerator DestroyAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null) Destroy(obj);
    }


    //// [NEW] Animation Methods For Molecules ////////////////////////////////////////////////////////////////////////////
    
    //Spawn Anim Method. Necessary incase different molecules end up with different spawn animation.
    private void PlaySpawnAnim(GameObject obj){ 

        Animator animator = obj.GetComponent<Animator>();

        if (animator != null){
            animator.Play("spawn");
            animator.Play("glucoseSpawn");            
        }
        
        StartCoroutine(FadeIn(obj)); //Calls in fade animation from below
    }

    //Exit Anim Method.
    private void PlayExitAnim(GameObject obj){ 

        Animator animator = obj.GetComponent<Animator>();

        if (animator != null){
            animator.Play("glucoseProcessed");            
        }        
    }

    //[NEW] Fade in animation for Molecule Entry//
       private IEnumerator FadeIn(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>(); //Gets the rendered because we modify Alpha Value
        if (renderer != null)
        {
            Color color = renderer.material.color;
            float duration = 0.25f;
            float elapsed = 0f;
            color.a = 0f; //a is alpha value, we initially set it to 0, then the code below takes it back to 1.
            renderer.material.color = color;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsed / duration);
                renderer.material.color = color;
                yield return null;
            }
        }
    }
}
