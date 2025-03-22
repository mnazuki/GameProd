using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework.Internal;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private DialogueContainer _dialogueContainer = new DialogueContainer();
    private List<DialogueLine> dialogueLines;
    private int currentIndex = 0;

    //private string jsonPath;
    [SerializeField] private TextAsset json; //Set dialogue json per trigger



    void Start()
    {

        //jsonPath = Path.Combine(Application.dataPath, "Dialog Stuff/Lines/td.json");
        dialogueLines = new List<DialogueLine>();

        LoadDialogue(json.text);
        DisplayDialogue();
    }


    void LoadDialogue(string jsonText)
    {
        if (jsonText!=null)
        {

            //string jsonText = File.ReadAllText(jsonPath);
            
            DialogueData data = JsonUtility.FromJson<DialogueData>(jsonText);
            
            
            dialogueLines.Clear();

            foreach (var entry in data.dialogues){
                DialogueCharacter character = new DialogueCharacter { name = entry.character, icon = LoadSprite(entry.sprite)};
                DialogueLine line = new DialogueLine { character = character, line = entry.text};
                dialogueLines.Add(line);

            }
        }
        else{
            Debug.LogError("Dialogue JSON file not found");
        }
    }

     Sprite LoadSprite(string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName))
        {
            Debug.LogWarning("Sprite name is empty, returning null.");
            return null;
        }

        // Load from Resources/Dialogue Stuff/Characters/
        Sprite loadedSprite = Resources.Load<Sprite>($"Dialog Stuff/Character/{spriteName}");

        if (loadedSprite == null)
        {
            Debug.LogError($"Sprite not found: {spriteName}");
        }

        return loadedSprite;
    }

    public void DisplayDialogue(){
        if (currentIndex < dialogueLines.Count)
        {
            DialogueLine line = dialogueLines[currentIndex];
            _dialogueContainer.nameText.text = line.character.name;
            _dialogueContainer.dialogueText.text = line.line;            
            _dialogueContainer.chSprite.sprite = line.character.icon;
            currentIndex++;
        }else{
            Debug.Log("End of Dialogue");
        }
    }

    public void OnNextButtonPressed(){
        DisplayDialogue();
    }
}

