using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{ 
    [SerializeField] private Button _fullscreenButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _settingsPanel;
    private FMOD.Studio.Bus _busMusic;
    private FMOD.Studio.Bus _busSound;
    public string _busMusicName;
    public string _busSoundName;


    private bool _isFullscreen;
    private bool _isShowSettings;

    [SerializeField] private EventReference _buttonPushSound;


    private void Start()
    {
        _busMusic = FMODUnity.RuntimeManager.GetBus("bus:/" + _busMusicName);
        _busSound = FMODUnity.RuntimeManager.GetBus("bus:/" + _busSoundName);
        _fullscreenButton.onClick.AddListener(() => { SetFullScreen(); });
    }

    public void SetVolumeMusic(int value)
    {
        _busMusic.setVolume(value);
    }  
    
    public void SetVolumeSound(int value)
    {
        _busSound.setVolume(value);
    }

    public void ChooseLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetFullScreen()
    {
        _isFullscreen = !_isFullscreen;
        Screen.fullScreen = _isFullscreen;
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
