using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour
{
    // Reference to Text component
    Text _text;

    // Reference to Dialogue System within the scene
    DialogueSystem _dialogueSystem;

    // Full dialogue message
    string _dialogue;

    // Position within the text string, for animating
    int _textPosition;

    public bool fullMessageShown
    {
        get
        {
            return _textPosition >= _dialogue.Length;
        }
    }

    public void Awake()
    {
        // Get reference to Text component
        _text = GetComponent<Text>();
        if (_text == null)
        {
            Debug.LogError(string.Format("({0}) Unable to find Text component on this object", this.name));
        }

        // Get reference to Dialogue System
        _dialogueSystem = FindObjectOfType<DialogueSystem>();
        if (_dialogueSystem == null)
        {
            Debug.LogError(string.Format("({0}) Unable to find Dialogue System within scene", this.name));
        }

        // Save text within message
        _dialogue = _text.text;
    }

    void Update()
    {

    }

    public void OnEnable()
    {
        // Used to prevent prefabs from calling Invokes
        if (transform.parent == null)
        {
            return;
        }

        // Reset dialogue string, and position within text
        _textPosition   = 0;

        // Began animation of text
        InvokeRepeating("AnimateText", 0.0f, _dialogueSystem.textSpeed);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }

    void AnimateText()
    {
        // Show text
        _text.text = _dialogue.Substring(0, _textPosition);

        if (_textPosition < _dialogue.Length)
        {
            Debug.Log(string.Format("({0}) Animating text", this.name));
            _textPosition++;
        }
        else
        {
            Debug.Log(string.Format("{0} is displaying end of dialogue prompt", this.name));

            // Display End of Dialogue Prompt
            _dialogueSystem.DisplayEndOfDialoguePrompt();

            CancelInvoke();
        }
    }

    public void ShowFullMessage()
    {
        _textPosition = _dialogue.Length;

        CancelInvoke();
        AnimateText();
    }
}
