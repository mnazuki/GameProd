using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using NUnit.Framework.Internal;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class DialogueSystem : MonoBehaviour
{
    [Header("UI ASSIGNMENT")]
    [SerializeField] private GameObject dialogueBox; //The Scene's Dialogue Set, including the Box, Sprite, Texts & Buttons
    [SerializeField] private DialogueContainer _dialogueContainer = new DialogueContainer(); //The space where the text goes.
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button autoButton;
    [SerializeField] private Sprite autoOn;
    [SerializeField] private Sprite autoOff;

    [Header("AUDIO & TEXT")]
    [SerializeField] private AudioSource audioSource; // 🎵 Typing sound source
    [SerializeField] private AudioClip typingSound; // 🎵 Sound clip
    [SerializeField] private TextAsset json; //Set dialogue json per trigger. Drag & Drop in inspector

    private List<DialogueLine> dialogueLines;
    private Coroutine typing;
    private int currentIndex = 0;   
    private bool isTyping = false;
    private bool isAutoMode = false;


    void Start()
    {

        //FInds & Assigns The NEXT Button.
        if (nextButton == null){ nextButton = GameObject.Find("Next")?.GetComponent<Button>(); }
        if ( nextButton != null){ nextButton.onClick.AddListener(OnNextButtonPressed); } 
        else { Debug.LogError("Next Button not found!"); }

        //FInds & Assigns The SKIP Button.
        if (skipButton == null){ skipButton = GameObject.Find("Skip")?.GetComponent<Button>(); }
        if ( skipButton != null){ skipButton.onClick.AddListener(OnSkipButtonPressed); } 
        else { Debug.LogError("Skip Button not found!"); }

        //FInds & Assigns The AUTO Button.
        if (autoButton == null) { autoButton = GameObject.Find("Auto")?.GetComponent<Button>(); }
        if (autoButton != null) { autoButton.onClick.AddListener(ToggleAutoMode); }
        else { Debug.LogError("Auto Button not found!"); }

        //Activates
        dialogueBox.SetActive(true);
        dialogueLines = new List<DialogueLine>();
        
        Debug.Log($"Index: {currentIndex}");
       
        LoadDialogue(json.text);
        DisplayAllDialogueItems(); //For debugging
        DisplayDialogue();
        
    }

    void Update()
    {
         // Change the sprite based on the isAutoMode flag
        Sprite autoSprite = isAutoMode ? autoOn : autoOff;
        autoButton.GetComponent<Image>().sprite = autoSprite;
    }

    //////// Loads the entries in the JSON into the list created above
    void LoadDialogue(string jsonText)
    {
        if (jsonText!=null)
        {
            DialogueData data = JsonUtility.FromJson<DialogueData>(jsonText);             
            dialogueLines.Clear();

            //For each entry in json, load it into their respective spots in each line of the list.
            foreach (var entry in data.dialogues){
                DialogueCharacter character = new DialogueCharacter { name = entry.character, icon = LoadSprite(entry.sprite, entry.sID)};
                DialogueLine line = new DialogueLine { character = character, line = entry.text};
                dialogueLines.Add(line);
            }}
        else{ Debug.LogError("Dialogue JSON file not found");}
    }


    //////// [Sprite Manager]
     Sprite LoadSprite(string spriteName, string spriteID)
    {
        // Load from Assets/Resources/Dialogue Stuff/Characters/ [Important that the assets are in the main "Resources" folder]
        Sprite[] allSprites = Resources.LoadAll<Sprite>($"Dialog Stuff/Character/{spriteName}");
        Sprite loadedSprite = Array.Find(allSprites, sprite => sprite.name == $"{spriteName}_{spriteID}");

        Debug.Log($"Sprite: {spriteName + spriteID}"); 

        if (loadedSprite == null)
        { Debug.LogError($"Sprite not found: {spriteName}"); }

        return loadedSprite;
    }


    //////// [Displays the dialogue]
    public void DisplayDialogue(){
        if (currentIndex < dialogueLines.Count)
        {            
            Debug.Log($"currentIndex: {currentIndex}");

            //Passes the items in dialogueLines to the list to be displayed.
            DialogueLine line = dialogueLines[currentIndex]; 
            _dialogueContainer.nameText.text = line.character.name;        
            _dialogueContainer.chSprite.sprite = line.character.icon;

            //If there is text still typing, stop it so we can go to the next line.
            if (typing != null){
                StopCoroutine(typing);}

            //Renders the next line of text
            typing = StartCoroutine(TypeText(line.line));

            }else{
            
            Debug.Log("End of Dialogue");
            Debug.Log($"Index: {currentIndex},Count: {dialogueLines.Count}");
            isAutoMode = false;
            dialogueBox.SetActive(false);
            Destroy(this.gameObject); //IMPORTANT. For some reason w/o this, multiple dialogue sets won't work properly. So any other script that checks for this must always have a null check to prevent log flooding.
        }
    }


    //////// [For the "Per Letter" Text Typing Animation]
    IEnumerator TypeText(string text){
        isTyping = true;
        _dialogueContainer.dialogueText.text = "";

        foreach (char letter in text){
            _dialogueContainer.dialogueText.text += letter;

            //Text Beeps. If you want variations in the sound pitch per letter, play around with the Random values.
            if (audioSource != null && typingSound != null){
                audioSource.pitch = Random.Range(1f, 1f); 
                audioSource.PlayOneShot(typingSound);
            }

            // Seconds to wait before next letter appears. [Sidenote: Using a variable doesn't seem to work properly here. Just adjust it here directly for now]
            yield return new WaitForSeconds(0.028f); 
        }
        isTyping = false;
        currentIndex++;

        ////Checks if Auto Button is Toggled & automates displaying the next line if it is.
        if (isAutoMode)
        {
            // Seconds to wait before next dialogue loads. [Sidenote: Same issue as above. Just change here directly]
            yield return new WaitForSeconds(1f); 
            DisplayDialogue();
        }
    }


    //////// [Next & Skip & Auto Button Functions]
    public void OnNextButtonPressed(){
        
        //Checks if text is still typing, so when NEXT is pressed it will skip & load the next line.
        if (isTyping){
            StopCoroutine(typing);
            _dialogueContainer.dialogueText.text = dialogueLines[currentIndex].line;
            isTyping = false;
            currentIndex++;
        }        
        DisplayDialogue();
    }

    public void OnSkipButtonPressed(){
         Debug.Log("End of Dialogue");
         isAutoMode = false;
         dialogueBox.SetActive(false);
         Destroy(this.gameObject);
    }

     public void ToggleAutoMode()
    {
        isAutoMode = !isAutoMode;
        Debug.Log("Auto Mode: " + (isAutoMode ? "ON" : "OFF"));

        if (isAutoMode && !isTyping)
        { DisplayDialogue(); }
    }


    //////// [For Debugging]
    void DisplayAllDialogueItems()
    {
        if (dialogueLines != null && dialogueLines.Count > 0)
        {
            foreach (var line in dialogueLines)
            {
                Debug.Log($"Character: {line.character.name}, Dialogue: {line.line}");
                Debug.Log($"Count: {dialogueLines.Count}");
            }
        }
        else
        { Debug.LogWarning("No dialogue lines found!"); }
    }

}

