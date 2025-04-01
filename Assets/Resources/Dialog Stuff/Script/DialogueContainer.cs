using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Used in DialogueSystem.cs. Then it is assigned the "root container" game object in the Dialogue Box group in the hierarchy. 
// This is used to display the text.

[System.Serializable]
public class DialogueContainer 
{
    public GameObject root;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public UnityEngine.UI.Image chSprite;

}
