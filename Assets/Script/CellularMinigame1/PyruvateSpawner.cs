// placed on Game Manager Spawner at the hierarchy (add component)
// used by button to start oxidation (on click)

using UnityEngine;
using TMPro;

public class PyruvateSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject miniPyruvate1Prefab;
    [SerializeField] private GameObject miniPyruvate2Prefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform miniPyruvate1SpawnPoint;
    [SerializeField] private Transform miniPyruvate2SpawnPoint;

    private bool hasSpawnedMiniPyruvates = false;
    private int collectedATP = 0;
    private int collectedNADH = 0;
    private int collectedMP = 0; //[NEW] Keeps track of mini pyruvate 
    private const int requiredCollections = 2; // must collect 2 ATP and 2 NADH

    public static PyruvateSpawner Instance { get; private set; } // singleton instance to access the method
    public TextMeshProUGUI ATPcounter;
    public TextMeshProUGUI NADHcounter;

    private void Awake()
    {
        // ensures that only one instance of PyruvateSpawner exists in the scene, is a must when using singleton (getter, setter)
        if (Instance == null) {
            Instance = this;
        }
        else{ 
            Destroy(gameObject);
        } 
    }

    //[NEW] Update function responsible for Monitor Screen Display
    private void Update(){ 
        //[NEW] If mini pyruvates haven't spawned, it means either cycle is ongoing or failed, therefore no Net Yield display. 
        if (!hasSpawnedMiniPyruvates){ 

            NADHcounter.fontSize = 21f;
            ATPcounter.text = "ATP: " + collectedATP + "/2";
            NADHcounter.text = "NADH: " + collectedNADH + "/2";

        }

        //[NEW] If mini pyruvates have spawned, it means the cycle is a success, therefore displaying the Net Yield.
        else{ 
            NADHcounter.fontSize = 18f;
            ATPcounter.text = "NET YIELD";
            NADHcounter.text = "2 ATP\n2 NADH\n2 Pyruvate";
            
        }

    }

    // called when ATP or NADH is collected
    public void CollectMolecule(string moleculeType)
    {
        if (moleculeType == "ATP"){
            // increase the collected ATP count
            collectedATP++;
           
        } 
        else if (moleculeType == "NADH"){
            // incerease the collected NADH count
            collectedNADH++;
        } 

        // Debug.Log($"ATP Collected: {collectedATP}/2, NADH Collected: {collectedNADH}/2");
    }

    // check if all ATP and NADH are collected
    // used in Pyruvate.cs
    public bool AreAllMoleculesCollected()
    {
        return collectedATP >= requiredCollections && collectedNADH >= requiredCollections;
    }


    public void SpawnMiniPyruvates()
    {
        // check if mini pyruvates have already spawned
         if (!hasSpawnedMiniPyruvates) // only spawn if not already spawned (since this script is added in 2 prefabs)
        {
            Instantiate(miniPyruvate1Prefab, miniPyruvate1SpawnPoint.position, Quaternion.identity);
            Instantiate(miniPyruvate2Prefab, miniPyruvate2SpawnPoint.position, Quaternion.identity);
            hasSpawnedMiniPyruvates = true; // set flag to true to prevent multiple spawns
            // Debug.Log("Mini pyruvates spawned!");
        }
    }

    // resets collection count when a new glucose is spawned to prevent continous collection of ATP and NADH over 2
    public void ResetCollection()
    {
        collectedATP = 0;
        collectedNADH = 0;
        hasSpawnedMiniPyruvates = false; // Allow mini pyruvates to spawn again
        // Debug.Log("ATP and NADH collection reset!");
    }

    // used in Start Oxidation button (on click)
    public void DestroyMiniPyruvate(){
        
        if (GameObject.FindWithTag("MiniPyruvate") != null){
            Destroy(GameObject.FindWithTag("MiniPyruvate"));

            //Keeps track of the # of successful cycles. 2 Mini Pyru [MP] = 1 Cycle. Win condition is here too.
            collectedMP++;
        }        
    }

    public int showCollectedMP(){
        return collectedMP;
    }
}
