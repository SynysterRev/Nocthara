using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public delegate void TypeWriterDelegate();
    public event TypeWriterDelegate OnTypeWriterEnds;
    
    
    [SerializeField] 
    private GameObject TypeWriterPrefab;
    
    private TypeWriter _typeWriter;

    private List<Dialogue> _dialogues;
    private int _currentDialogueIndex;
    private int _numberDialogues;
    private bool _isDialogueRunning;

    [SerializeField]
    private DialogueInputManager DialogueInput;
    
    private PlayerController _playerController;

    private void OnEnable()
    {
        DialogueInput.EnableInput(true);
    }

    private void OnDisable()
    {
        DialogueInput.EnableInput(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!_playerController)
        {
            _playerController = PlayerManager.Instance.PlayerController;
        }
        BindInput(true);
        DialogueInput.EnableInput(false);
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

    private void InitDialogue()
    {
        if (_playerController != null)
        {
            _playerController.EnablePlayerInput(false);
        }
        if (DialogueInput)
        {
            DialogueInput.EnableInput(true);
        }
        _currentDialogueIndex = 0;

        if (_typeWriter == null && TypeWriterPrefab)
        {
            GameObject parent = GameObject.FindGameObjectWithTag("UICanvas");
            _typeWriter = Instantiate(TypeWriterPrefab, parent.transform).GetComponent<TypeWriter>();
        }
    }

    private void StartTypeWriter(string firstDialogue, string characterName, Sprite insert=null)
    {
        if (_typeWriter)
        {
            _typeWriter.gameObject.SetActive(true);
            _typeWriter.OnTypeWriterEnd += OnTypeWriterEnd;
            _isDialogueRunning = true;
            _typeWriter.StartTypeWriter(firstDialogue,characterName);
            if (insert)
            {
                _typeWriter.DisplayInsert(insert);
            }
        }
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        InitDialogue();
        _dialogues = dialogues;
        _numberDialogues = _dialogues.Count;
        StartTypeWriter(_dialogues[_currentDialogueIndex].DialogueText,
            _dialogues[_currentDialogueIndex].CharacterName);
    }

    public void StartOneDialogue(string dialogueText, Sprite insert=null)
    {
        InitDialogue();
        _numberDialogues = 1;
        StartTypeWriter(dialogueText, "", insert);
    }

    private void EndDialogue()
    {
        _dialogues = null;
        _currentDialogueIndex = 0;
        _isDialogueRunning = false;
        if (_typeWriter)
        {
            _typeWriter.HideInsert();
            _typeWriter.OnTypeWriterEnd -= OnTypeWriterEnd;
            _typeWriter.gameObject.SetActive(false);
        }

        if (DialogueInput)
        {
            DialogueInput.EnableInput(false);
        }

        if (_playerController != null)
        {
            _playerController.EnablePlayerInput(true);
        }
        OnTypeWriterEnds?.Invoke();
    }

    private void OnTypeWriterEnd()
    {
        _isDialogueRunning = false;
    }

    private void OnInteract()
    {
        if (_isDialogueRunning && _typeWriter)
        {
            _typeWriter.SkipToEnd();
            return;
        }
        _currentDialogueIndex++;
        if (_currentDialogueIndex >= _numberDialogues)
        {
            EndDialogue();
            return;
        }
        if (_typeWriter)
        {
            _isDialogueRunning = true;
            _typeWriter.StartTypeWriter(_dialogues[_currentDialogueIndex].DialogueText,
                _dialogues[_currentDialogueIndex].CharacterName);
            if(_dialogues[_currentDialogueIndex].Insert)
                _typeWriter.DisplayInsert(_dialogues[_currentDialogueIndex].Insert);
            else
            {
                _typeWriter.HideInsert();
            }
        }
    }
}