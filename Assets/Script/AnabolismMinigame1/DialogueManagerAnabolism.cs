using UnityEngine;

public class DIalogueManagerAnabolism : MonoBehaviour
{

// DIALOGUE MANAGER for [Electron Transport Chain]. This is not a constant among minigames, each game has different setup.
    public GameObject  spawner, d1;

    
    void Start()
    {
        if (d1.activeInHierarchy == false){
            d1.SetActive(true);
        }   
    }

    void Update()
    {
       if (d1 == null){
        spawner.SetActive(true);
       } 
    }
}
