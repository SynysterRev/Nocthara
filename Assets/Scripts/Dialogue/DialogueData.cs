using System;
using System.Collections.Generic;
using UnityEngine;

public class MaxStringLengthAttribute : PropertyAttribute
{
    public int MaxLength { get; }

    public MaxStringLengthAttribute(int maxLength)
    {
        MaxLength = maxLength;
    }
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    // // [MaxStringLength(20)]
    // public string CharacterName;
    // // [TextArea(1, 3)]
    // [MaxStringLength(150)]
    // public string DialogueText;
    public List<Dialogue> Dialogues = new List<Dialogue>();
}

[Serializable]
public struct Dialogue
{
    public string CharacterName;
    [MaxStringLength(150)]
    public string DialogueText;
}
