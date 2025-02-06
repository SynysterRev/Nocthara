using System.Collections.Generic;
using UnityEngine;

public class Npc : InteractObject
{
    [SerializeField] 
    private List<DialogueData> DialogueList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject player, GameObject target)
    {
        if (DialogueList.Count == 0)
        {
            Debug.LogError("Aucun dialogue pour le PNJ " + gameObject.name);
            return;
        }
        
        DialogueManager.Instance.StartDialogue(DialogueList[0].CharacterName, DialogueList[0].DialogueText);
    }
}
