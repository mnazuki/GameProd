using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    
    // [GLYCOLYSIS DIALOGUE MANAGER]
    // [SIDENOTE]: This is not a necessary component in the particular minigame. However, I still put this here as it still can be useful in more complex dialogue triggers. To use in other minigames, it must be copy pasted into the respective minigame's own scripts folder.

    //Please see comment in the Start() function.

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

        //// [Dialogue Trigger] : When Dialogue Manager is loaded. This can be susbstituted by enabling D1 in the hierarchy before play instead.
        d1.SetActive(true); //This is the only thing used here. The code above & below are suggested templates for use in other scenes.
    }

    public void triggerDialogue(string d){
        dialogueDict[d].SetActive(true);
    }

}
