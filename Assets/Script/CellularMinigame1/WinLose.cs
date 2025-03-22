using System;
using UnityEngine;
using TMPro;

//[NEW SCRIPT] Game Manager for Win & Lose Conditions
public class WinLose : MonoBehaviour
{
     
    [SerializeField] private int requiredCycles; //Change in inspector or Start(). Initializing here does weird activities.
    
    
    public TextMeshProUGUI cyclesText;
    public TextMeshProUGUI ATPtext;
    public WinScreen winscreen;
    public Gameover gameover;
    public PyruvateSpawner pyru;
    public HealthManager hp;
    public EnergyBar energy;
    public Spawning energyReq;
    //public DialogueManager dialogueManager;
    public GameObject d2;

    

    private int cycles = 0;
    private int previousCycles;

    void Update()
    {
            //WIN CONDITION: Player has successfully ////////////////////
            cycles = pyru.showCollectedMP() / 2; //2 Pyru = 1 Successful Cycle
            cyclesText.text = ("Glucose Processed: " + cycles + "/" + requiredCycles); //Shows player's progress to Win

            if (cycles > previousCycles){
                ATPtext.text = "Total ATP: " + pyru.showCollectedMP();
                previousCycles = cycles;
            }

            
            if (cycles == requiredCycles){
                d2.SetActive(true);
                //winscreen.win();//Change to WIN
                
            }    

            //LOSE CONDITIONS //////////////////////////////////////////

            //Lose Condition 1: Out of HP
            if (hp.HealthCheck() == true){
                gameover.gmOver();
            }

            //Lose Condition 2: Out of Energy
            if (energyReq.emptyEnergy == true){
                gameover.gmOver();              
            }
    }

}
