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
    [SerializeField] private GameObject dialogueBox; //The Scene's Dialogue Set, including the Box, Sprite, Texts & Buttons
    [SerializeField] private DialogueContainer _dialogueContainer = new DialogueContainer(); //The space where the text goes.
    private List<DialogueLine> dialogueLines;
    private int currentIndex = 0;
    [SerializeField] private TextAsset json; //Set dialogue json per trigger. Drag & Drop in inspector
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button autoButton;
    [SerializeField] private AudioSource audioSource; // 🎵 Typing sound source
    [SerializeField] private AudioClip typingSound; // 🎵 Sound clip

    //[SerializeField] private float typeSpeed = 0.1f; [For some reason this works very inconsistently. For now just change the value directly below in the TypeText Coroutine.]

    private Coroutine typing;
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

    //////// [Loads the entries in the JSON into the list created above]
    void LoadDialogue(string jsonText)
    {
        if (jsonText!=null)
        {
            DialogueData data = JsonUtility.FromJson<DialogueData>(jsonText);
            
            dialogueLines.Clear();

            foreach (var entry in data.dialogues){
                DialogueCharacter character = new DialogueCharacter { name = entry.character, icon = LoadSprite(entry.sprite)};
                DialogueLine line = new DialogueLine { character = character, line = entry.text};
                dialogueLines.Add(line);
            }}
        else{ Debug.LogError("Dialogue JSON file not found");}
    }


    //////// [Sprite Manager]
     Sprite LoadSprite(string spriteName)
    {
        // Load from Assets/Resources/Dialogue Stuff/Characters/ [Important that the assets are in the main Resources folder]
        Sprite loadedSprite = Resources.Load<Sprite>($"Dialog Stuff/Character/{spriteName}");

        if (loadedSprite == null)
        { Debug.LogError($"Sprite not found: {spriteName}"); }

        return loadedSprite;
    }


    //////// [Displays the dialogue]
    public void DisplayDialogue(){
        if (currentIndex < dialogueLines.Count)
        {            
            Debug.Log($"currentIndex: {currentIndex}");
            DialogueLine line = dialogueLines[currentIndex]; //Passes the items in dialogueLines to the list to be displayed.
            _dialogueContainer.nameText.text = line.character.name;
            //_dialogueContainer.dialogueText.text = line.line;            
            _dialogueContainer.chSprite.sprite = line.character.icon;

            if (typing != null){
                StopCoroutine(typing);
            }

            typing = StartCoroutine(TypeText(line.line));

                   }else{
            Debug.Log("End of Dialogue");
            Debug.Log($"Index: {currentIndex},Count: {dialogueLines.Count}");
            dialogueBox.SetActive(false);
            Destroy(this.gameObject); //IMPORTANT. For some reason w/o this, multiple dialogue sets won't work properly. So any other script that checks for this must always have a null check to prevent log flooding.
        }
    }

    //////// [For the Text Typing Animation]
    IEnumerator TypeText(string text){
        isTyping = true;
        _dialogueContainer.dialogueText.text = "";

        foreach (char letter in text){
            _dialogueContainer.dialogueText.text += letter;

            //Text Beeps
            if (audioSource != null && typingSound != null){
                audioSource.pitch = Random.Range(1f, 1f); //If you want variations in the sound pitch per letter, play around with these values.
                audioSource.PlayOneShot(typingSound);
            }
            yield return new WaitForSeconds(0.028f); //[Sidenote: Using a variable doesn't seem to work properly here. Just adjust it here directly for now]
        }

        isTyping = false;
        currentIndex++;

        //
        if (isAutoMode)
        {
            yield return new WaitForSeconds(1f); // Seconds to wait before next dialogue loads. [Sidenote: Same issue as above. Just change here directly]
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
         dialogueBox.SetActive(false);
         Destroy(this.gameObject);
    }

     public void ToggleAutoMode()
    {
        isAutoMode = !isAutoMode;
        Debug.Log("Auto Mode: " + (isAutoMode ? "ON" : "OFF"));

        Color autoColor = isAutoMode ? Color.black : Color.white;
        autoButton.GetComponent<Image>().color = autoColor;

        if (isAutoMode && !isTyping)
        {
            DisplayDialogue();
        }
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

