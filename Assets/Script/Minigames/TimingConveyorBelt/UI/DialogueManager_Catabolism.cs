using UnityEngine;

public class DIalogueManagerCatabolism : MonoBehaviour
{

// DIALOGUE MANAGER for [Electron Transport Chain]. This is not a constant among minigames, each game has different setup.
    public GameObject  gameManager, needle, d1;

    
    void Start()
    {
        if (d1.activeInHierarchy == false){
            d1.SetActive(true);
        }   
    }

    void Update()
    {
       if (d1 == null){
        gameManager.SetActive(true);
        needle.SetActive(true);
       } 
    }
}
