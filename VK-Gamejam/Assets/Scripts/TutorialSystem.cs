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

    [SerializeField] private float _typingSpeed;
    [SerializeField] private Button _contunieButton;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject[] _pointers;
    [SerializeField] private Image[] _boxes;

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
        foreach (char letter in _sentences[index].ToCharArray())
        {
            RuntimeManager.PlayOneShot(text);
            _textDisplay.text += letter;
            //yield return new WaitForSeconds(_typingSpeed);
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

                if (_boxes.Length > 0)
                {
                    foreach (Image box in _boxes)
                    {
                        box.raycastTarget = true;
                    }
                }

                if (_pointers.Length > 0)
                    _pointers[0].SetActive(true);
            }
        }
    }
}

