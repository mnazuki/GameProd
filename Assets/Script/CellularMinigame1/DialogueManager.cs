using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    
    //[GLYCOLISIS DIALOGUE MANAGER]

    //Triggers

    [SerializeField] private GameObject d1; //dialogue 1... 2 3 etc
    [SerializeField] private GameObject d2;
    private Dictionary<string, GameObject> dialogueDict;

    void Start()
    {
        dialogueDict = new Dictionary<string, GameObject>()
        {
            { "d1", d1 },
            { "d2", d2 }
        };

        d1.SetActive(true);
    }

    public void triggerDialogue(string d){
        dialogueDict[d].SetActive(true);
    }

}
