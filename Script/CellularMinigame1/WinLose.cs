using System;
using UnityEngine;
using TMPro;

//[NEW SCRIPT] Game Manager for Win & Lose Conditions
public class WinLose : MonoBehaviour
{
     
    [SerializeField] private int requiredCycles; //Change in inspector or Start(). Initializing here does weird activities.
    
    [Header("UI")]
    public TextMeshProUGUI cyclesText;
    public TextMeshProUGUI ATPtext;
    public TextMeshProUGUI gameOverText;
    public WinScreen winscreen;
    public Gameover gameover;
    public PyruvateSpawner pyru;
    public HealthManager hp;
    public EnergyBar energy;

     [Header("CONDITIONS")]
    public Spawning energyReq; //Required Energy to Win. Changable in Inspector.
    private int cycles = 0;
    private int previousCycles;

    [Header("DIALOGUE TRIGGERS")]
    public GameObject d2;
    public GameObject d3; 
    public GameObject d4;
    public GameObject d5;

    [Header("Minigame BGM & SFX")]
    public AudioSource bgmsrc;
    public AudioSource sfx;
    public AudioClip bgm, rumbling, machinefail;
    public GameObject sfxTrigger;

    void Start()
    {
     bgmsrc.clip = bgm;
     bgmsrc.loop = true;
     bgmsrc.Play();   
    }

    void Update()
    {
            //// [[WIN CONDITION]] : Player has successfully collected enough Mini Pyru
            cycles = pyru.showCollectedMP() / 2; //2 Pyru = 1 Successful Cycle
            cyclesText.text = "Glucose Processed: " + cycles + "/" + requiredCycles; //Shows player's progress to Win

            if (cycles > previousCycles){
                ATPtext.text = "Total ATP: " + pyru.showCollectedMP();
                previousCycles = cycles;
            }

            //// [Dialogue Trigger] : When player is halfway to victory.
            if (cycles == requiredCycles/2){
             if (d3 != null){
                d3.SetActive(true);
            }}

            //// [Dialogue Trigger & WinScreen] : When player wins.
            if (cycles == requiredCycles){
                if (d2 != null){
                d2.SetActive(true);
                }
                if(d2 == null){
                    winscreen.win();//Change to WIN
            }}    

            // [[LOSE CONDITIONS]] //////////////////////////////////////////

            //Lose Condition 1: Out of HP
            if (hp.HealthCheck() == true){
                if(d4 != null){
                   d4.SetActive(true);
                   bgmsrc.Stop();
                   sfx.clip = rumbling;
                   sfxTrigger.SetActive(true);
                }
                if (d4 == null){
                gameOverText.text = "The System has been Infected";
                gameover.gmOver();
                sfx.Stop();
                }
            }

            //Lose Condition 2: Out of Energy
            if (energyReq.emptyEnergy == true){
                if(d5 != null){
                   d5.SetActive(true); 
                   bgmsrc.Stop();
                   sfx.clip = machinefail;
                   sfxTrigger.SetActive(true);
                }
                if (d5 == null){
                gameOverText.text = "Out of Energy";
                gameover.gmOver();
                sfx.Stop();
                }             
            }
    }

}
