using System.Collections.Generic;
using UnityEngine;

//Used in DialogueSystem.cs. This contains the data format for the dialogue lines so we can transfer the entries in JSON to a list.

[System.Serializable]
public class DialogueEntry{
    public int id;
    public string character;
    public string text;
    public string sprite;
    public string sID;
}

[System.Serializable]
public class DialogueData
{
    public List<DialogueEntry> dialogues;
}
