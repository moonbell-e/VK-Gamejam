using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class MenuSystem : MonoBehaviour
{ 
    [SerializeField] private Button _fullscreenButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _settingsPanel;

    private bool _isFullscreen;
    private bool _isShowSettings;

    [SerializeField] private EventReference _buttonPushSound;


    private void Start()
    {
        _fullscreenButton.onClick.AddListener(() => { SetFullScreen(); });
    }



    public void SetFullScreen()
    {
        _isFullscreen = !_isFullscreen;
        Screen.fullScreen = !_isFullscreen;
        PlayButtonPushSound();
    }

    public void IsShowSettings()
    {
        _isShowSettings= !_isShowSettings;
        _settingsPanel.SetActive(_isShowSettings);
        PlayButtonPushSound();
    }

    public void PlayButtonPushSound()
    {
        RuntimeManager.PlayOneShot(_buttonPushSound);
    }
}
