using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextTag
{
    public enum TagType { Text, OpenTag, CloseTag }
    public TagType Type;
    public string Value;
    
    public TextTag(TagType type, string value)
    {
        Type = type;
        Value = value;
    }
}

public class TypeWriter : MonoBehaviour
{
    public delegate void TypeWriterEnd();

    public event TypeWriterEnd OnTypeWriterEnd;

    [SerializeField] private float TextSpeed = 0.018f;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private Image Insert;
    private const string _alphaMark = "alpha=#00";
    private string _textToDisplay;
    private string _finalText;
    private float _numberCharacterToDisplay = 0f;
    private bool _updateText = false;
    private int _textLength = 0;
    private Coroutine _writingCoroutine;

    private List<TextTag> _textTags;

    private void Start()
    {
        if (Insert)
        {
            Insert.sprite = null;
            Insert.gameObject.SetActive(false);
        }

    }

    public void StartTypeWriter(string textToDisplay, string characterName)
    {
        CharacterName.text = characterName;
        _finalText = textToDisplay;
        _textLength = _finalText.Length;
        _textToDisplay = "";
        _numberCharacterToDisplay = 0;
        ParseText();
    }

    public void DisplayInsert(Sprite sprite)
    {
        if (Insert)
        {
            Insert.sprite = sprite;
            Insert.gameObject.SetActive(true);
        }
    }

    public void HideInsert()
    {
        if (Insert)
        {
            Insert.sprite = null;
            Insert.gameObject.SetActive(false);
        }
    }

    private void ParseText()
    {
        //m_TextToDisplay = string.Format("<{0}>{1}", c_AlphaMark, m_FinalText);
        //if (m_TxtMeshPro)
        //{
        //    m_TxtMeshPro.SetText(m_TextToDisplay);
        //}
        
        _textTags = new List<TextTag>();
        for (int i = 0; i < _finalText.Length; ++i)
        {
            if (_finalText[i] == '<')
            {
                for (int j = i + 1; j < _finalText.Length; ++j)
                {
                    if (_finalText[j] == '>')
                    {
                        if (_finalText[i + 1] == '/')
                        {
                            string test = _finalText.Substring(i, j - i + 1);
                            _textTags.Add(new TextTag(TextTag.TagType.CloseTag, test));
                        }
                        else
                        {
                            string test = _finalText.Substring(i, j - i + 1);
                            _textTags.Add(new TextTag(TextTag.TagType.OpenTag, test));
                        }

                        i = j;
                        break;
                    }
                }
            }
            else
            {
                _textTags.Add(new TextTag(TextTag.TagType.Text, _finalText[i].ToString()));
                _textToDisplay += _finalText[i];
            }
        }
        _updateText = true;
        _writingCoroutine = StartCoroutine(StartWriting());
    }

    private IEnumerator StartWriting()
    {
        while (_updateText)
        {
            _numberCharacterToDisplay += Time.deltaTime / TextSpeed;
            if (_numberCharacterToDisplay >= _textLength)
            {
                EndDisplayText();
            }
            else
            {
                UpdateText();
            }

            yield return null;
        }
    }
    
    private string GetClosingTag(string openTag)
    {
        string closeTag = openTag.Split('=')[0];
        closeTag = closeTag.Insert(1, "/");
        return closeTag + ">";
    }

    private void UpdateText()
    {
        string visibleText = "";
        string hiddenText = "";
        Stack<string> stack = new Stack<string>();
        for (int i = 0; i < _textTags.Count; ++i)
        {
            if (i <= (int)_numberCharacterToDisplay)
            {
                visibleText += _textTags[i].Value;
                if (_textTags[i].Type == TextTag.TagType.OpenTag)
                {
                    stack.Push(_textTags[i].Value);
                }
                if (_textTags[i].Type == TextTag.TagType.CloseTag)
                {
                    stack.Pop();
                }
            }
            else if (i == (int)_numberCharacterToDisplay)
            {
                foreach (var toast in stack)
                {
                    // Debug.Log("pourquoi ça " + GetClosingTag(toast).ToString() + " marche pas");
                    visibleText += GetClosingTag(toast) + $"<{_alphaMark}>";
                }
                stack.Clear();
            }
            else
            {
                hiddenText += _textTags[i].Value;
                if (_textTags[i].Type != TextTag.TagType.Text)
                {
                    hiddenText +=$"<{_alphaMark}>";
                }
            }
        }
        // Debug.Log(test);
        _textToDisplay = $"{visibleText}<{_alphaMark}>{hiddenText}";
        // _textToDisplay =
        //     $"{_finalText.Substring(0, (int)_numberCharacterToDisplay)}<{_alphaMark}>{_finalText.Substring((int)_numberCharacterToDisplay)}";
        if (DialogueText)
        {
            DialogueText.SetText(_textToDisplay);
        }
    }

    public void SkipToEnd()
    {
        EndDisplayText();
    }

    private void EndDisplayText()
    {
        if (DialogueText)
        {
            DialogueText.SetText(_finalText);
        }

        _textTags.Clear();
        _updateText = false;
        OnTypeWriterEnd?.Invoke();
        StopCoroutine(_writingCoroutine);
    }
}