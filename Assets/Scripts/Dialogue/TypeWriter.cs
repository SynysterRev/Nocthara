using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TypeWriter : MonoBehaviour
{
    [FormerlySerializedAs("m_TextSpeed")] [SerializeField]
    private float _textSpeed = 0.018f;
    [SerializeField]
    private TextMeshProUGUI DialogueText;
    [SerializeField]
    private TextMeshProUGUI CharacterName;
    private const string _alphaMark = "alpha=#00";
    private string _textToDisplay;
    private string _finalText;
    private bool _skipTextAnim = false;
    private float _numberCharacterToDisplay = 0f;
    private bool _updateText = false;
    private int _textLength = 0;
    private Coroutine _writingCoroutine;

    // Update is called once per frame
    void Update()
    {

    }

    public void StartTypeWriter(string textToDisplay, string characterName)
    {
        CharacterName.text = characterName;
        _finalText = textToDisplay;
        _textLength = _finalText.Length;
        if(_skipTextAnim)
        {
            EndDisplayText();
            return;
        }
        ParseText();
       
    }

    private void ParseText()
    {
        //m_TextToDisplay = string.Format("<{0}>{1}", c_AlphaMark, m_FinalText);
        //if (m_TxtMeshPro)
        //{
        //    m_TxtMeshPro.SetText(m_TextToDisplay);
        //}

        _updateText = true;
        _writingCoroutine = StartCoroutine(StartWriting());
    }
    
    private IEnumerator StartWriting()
    {
        while (_updateText)
        {
            _numberCharacterToDisplay += Time.deltaTime / _textSpeed;
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

    private void UpdateText()
    {      
        _textToDisplay =
            $"{_finalText.Substring(0, (int)_numberCharacterToDisplay)}<{_alphaMark}>{_finalText.Substring((int)_numberCharacterToDisplay)}";
        if (DialogueText)
        {
            DialogueText.SetText(_textToDisplay);
        }
    }

    private void EndDisplayText()
    {
        if (DialogueText)
        {
            DialogueText.SetText(_finalText);
        }
        _skipTextAnim = false;
        _updateText = false;
        StopCoroutine(_writingCoroutine);
    }

}
