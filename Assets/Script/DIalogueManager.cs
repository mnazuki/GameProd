using UnityEngine;

public class DIalogueManager : MonoBehaviour
{

// DIALOGUE MANAGER for [Electron Transport Chain]. This is not a constant among minigames, each game has different setup.
    public GameObject spawner1, spawner2, d1;

    
    void Start()
    {
        if (d1.activeInHierarchy == false){
            d1.SetActive(true);
        }   
    }

    void Update()
    {
       if (d1 == null){
        spawner1.SetActive(true);
        spawner2.SetActive(true);
       } 
    }
}
