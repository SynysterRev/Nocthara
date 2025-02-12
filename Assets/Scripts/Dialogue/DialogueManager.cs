using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] 
    private TypeWriter TypeWriter;

    private List<Dialogue> _dialogues;
    private int _currentDialogueIndex;
    private bool _isDialogueRunning;

    [SerializeField]
    private DialogueInputManager DialogueInput;
    
    private PlayerController _playerController;

    private void OnEnable()
    {
        DialogueInput.EnableInput(true);
        if (!_playerController)
        {
            _playerController = PlayerManager.Instance.PlayerController;
        }
    }

    private void OnDisable()
    {
        DialogueInput.EnableInput(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BindInput(true);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BindInput(false);
    }

    private void BindInput(bool bind)
    {
        if (bind)
        {
            DialogueInput.InteractEvent += OnInteract;
        }
        else
        {
            DialogueInput.InteractEvent -= OnInteract;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        gameObject.SetActive(true);
        if (_playerController != null)
        {
            _playerController.EnablePlayerInput(false);
        }
        _dialogues = dialogues;
        _currentDialogueIndex = 0;
        if (TypeWriter)
        {
            TypeWriter.OnTypeWriterEnd += OnTypeWriterEnd;
            _isDialogueRunning = true;
            TypeWriter.StartTypeWriter(_dialogues[_currentDialogueIndex].DialogueText,
                _dialogues[_currentDialogueIndex].CharacterName);
        }
    }

    private void EndDialogue()
    {
        _dialogues = null;
        _currentDialogueIndex = 0;
        _isDialogueRunning = false;
        if (TypeWriter)
        {
            TypeWriter.OnTypeWriterEnd -= OnTypeWriterEnd;
        }
        if (_playerController != null)
        {
            _playerController.EnablePlayerInput(true);
        }
        gameObject.SetActive(false);
    }

    private void OnTypeWriterEnd()
    {
        _isDialogueRunning = false;
    }

    private void OnInteract()
    {
        if (_isDialogueRunning)
        {
            TypeWriter.SkipToEnd();
            return;
        }
        _currentDialogueIndex++;
        if (_currentDialogueIndex >= _dialogues.Count)
        {
            EndDialogue();
            return;
        }
        if (TypeWriter)
        {
            _isDialogueRunning = true;
            TypeWriter.StartTypeWriter(_dialogues[_currentDialogueIndex].DialogueText,
                _dialogues[_currentDialogueIndex].CharacterName);
        }
    }
}