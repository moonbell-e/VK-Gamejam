using System.Collections;
using UnityEngine;
using TMPro;
using FMODUnity;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textDisplay;

    [TextArea(2, 5)]
    [SerializeField] private string[] _sentences;

    [SerializeField] private Button _contunieButton;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _homeButton;
    [SerializeField] private GameObject[] _pointers;
    [SerializeField] private Box[] _boxes;

    [SerializeField] private EventReference text;

    private int index;

    private void Start()
    {
        if (_tutorialPanel.activeInHierarchy)
            Type();

        _contunieButton.onClick.AddListener(() => GoOnNextSentence());
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _tutorialPanel.SetActive(false);
            this.enabled = false;
        }
    }

    private void Type()
    {
        RuntimeManager.PlayOneShot(text);
        foreach (char letter in _sentences[index].ToCharArray())
        {
            _textDisplay.text += letter;
        }
    }

    public void GoOnNextSentence()
    {
        if (_textDisplay.text == _sentences[index])
        {
            if (index < _sentences.Length - 1)
            {
                index++;
                _textDisplay.text = "";
                Type();
            }
            else
            {
                _tutorialPanel.SetActive(false);
                if (_pointers.Length > 0)
                    _pointers[0].SetActive(true);
            }
        }
    }
}

