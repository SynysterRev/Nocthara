using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private TypeWriter TypeWriter;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(string characterName, string dialogueText)
    {
        gameObject.SetActive(true);
        if(TypeWriter)
            TypeWriter.StartTypeWriter(dialogueText, characterName);
    }
}
