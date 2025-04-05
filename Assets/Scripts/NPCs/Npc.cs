using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Npc : InteractObject
{
    public UnityEvent OnDialogueFinished;
    
    [SerializeField] 
    private DialogueData DialogueData;

    [SerializeField] 
    private Item ItemToGive;
    
    protected override void Interact(GameObject player, GameObject target)
    {
        if (DialogueData.Dialogues.Count == 0)
        {
            Debug.LogError("No dialogue for NPC " + gameObject.name);
            return;
        }
        
        DialogueManager.Instance.StartDialogue(DialogueData.Dialogues);
        DialogueManager.Instance.OnTypeWriterEnds += OnDialogueEnded;
    }

    private void OnDialogueEnded()
    {
        OnDialogueFinished?.Invoke();
    }
}
