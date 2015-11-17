using UnityEngine;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [Header("References")]
    [Tooltip(@"Children of this object will be treated as dialogue. 
        They will appear using the order within the hierachy")]
    [SerializeField]
    GameObject _dialogueList;

    [SerializeField]
    [Tooltip(@"This object will be shown as the end of each Dialogue message, 
        Can be used to notify the user how to advance the messages.")]
    GameObject _endOfMessagePrompt;

    [Header("Configuration")]
    [Tooltip(@"Controls the amount of time to pass before the next 
        character will be displayed")]
    [SerializeField]
    float _textSpeed = 0.025f;

    // Dialogue messages to display
    Dialogue[] _dialogueArray;

    // Current dialogue message, within the array
    int _currentMessageItr;

    // Cached GameObject properties
    GameObject _gameObject;

    public float textSpeed
    {
        get
        {
            return _textSpeed;
        }
    }

    public void Awake()
    {
        // Cache GameObject properties
        _gameObject = gameObject;

        // Initialise dialogue list array
        if (_dialogueList == null ||
            _dialogueList.transform.childCount < 1)
        {
            Debug.LogError(string.Format(@"({0}) Dialogue list not set correctly. 
                    Please ensure there is at least 1 child of the dialogue object",
                this.name));
        }
        else
        {
            // Get array of Dialogue
            GetDialogueList();

            // Ensure dialogue list is valid
            if (_dialogueArray == null ||
                _dialogueArray.Length <= 0)
            {
                Debug.LogWarning(string.Format("({0}) Unable to find any dialogue messages", this.name));
            }
        }
    }

    void GetDialogueList()
    {
        // Initiialise temporary list of all the dialogue messages
        List<Dialogue> dialogues =
            new List<Dialogue>(_dialogueList.transform.childCount);

        for (int i = 0; i < dialogues.Capacity; ++i)
        {
            // Get current child
            Transform dialogueTransform = _dialogueList.transform.GetChild(i);

            // Get Dialogue component, and save it to the list
            Dialogue dialogue = dialogueTransform.GetComponent<Dialogue>();
            dialogues.Add(dialogue);
        }

        // Save list as arry
        _dialogueArray = dialogues.ToArray();
    }

    void Update()
    {

    }

    public void OnEnable()
    {
        // Reset meessage interator, and show first message
        _currentMessageItr = 0;
        _dialogueArray[0].gameObject.SetActive(true);

        // Hide End of Dialogue Prompt, if any exist
        if (_endOfMessagePrompt)
        {
            _endOfMessagePrompt.SetActive(false);
        }
    }

    public void OnDisable()
    {
        // Hide all dialogue messages, so they correctly reset
        for (int i = 1; i < _dialogueArray.Length; ++i)
        {
            _dialogueArray[i].gameObject.SetActive(false);
        }
    }

    public void ShowNextMessage()
    {
        // Ignore if this component is disabled
        if (!this.enabled)
        {
            return;
        }

        // Show full message of current message, if still being animated
        if (!_dialogueArray[_currentMessageItr].fullMessageShown)
        {
            _dialogueArray[_currentMessageItr].ShowFullMessage();
            return;
        }

        // Hide current message
        _dialogueArray[_currentMessageItr].gameObject.SetActive(false);
        // Hide End of Dialogue Prompt
        if (_endOfMessagePrompt != null)
        {
            DisplayEndOfDialoguePrompt();
        }

        _currentMessageItr++;

        if (_currentMessageItr >= _dialogueArray.Length)
        {
            // End of messages, hide Dialogue system
            _gameObject.SetActive(false);
        }
        else
        {
            // Show next message
            _dialogueArray[_currentMessageItr].gameObject.SetActive(true);

            HideEndOfDialoguePrompt();
        }
    }

    public void DisplayEndOfDialoguePrompt()
    {
        // Display end of message prompt, if any exist
        if (_endOfMessagePrompt != null)
        {
            _endOfMessagePrompt.SetActive(true);
        }
    }

    void HideEndOfDialoguePrompt()
    {
        // Display end of message prompt, if any exist
        if (_endOfMessagePrompt != null)
        {
            _endOfMessagePrompt.SetActive(false);
        }
    }
}
