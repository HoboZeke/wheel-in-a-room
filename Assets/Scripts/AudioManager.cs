using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager main;

    [Range(0.0f, 1.0f)]
    [SerializeField] float masterVolume;
    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; OnValuesChange(); } }
    [Range(0.0f, 1.0f)]
    [SerializeField] float musicVolume;
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; OnValuesChange(); } }
    [Range(0.0f, 1.0f)]
    [SerializeField] float sfxVolume;
    public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; OnValuesChange(); } }

    [SerializeField] bool mute;
    public bool Mute { get { return mute; } set { mute = value; OnValuesChange(); } }

    public EventHandler<AudioEventArgs> OnAudioSettingsUpdate;

    [Header("UI")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider, sfxSlider;
    [SerializeField] Toggle muteToggle;

    [Header("Pause UI")]
    [SerializeField] Slider pauseMasterSlider;
    [SerializeField] Slider pauseMusicSlider, pauseSfxSlider;
    [SerializeField] Toggle pauseMuteToggle;

    [Header("Music")]
    [SerializeField] AudioPlayer menuBackground;
    [SerializeField] AudioPlayer gameBackground;
    [SerializeField] AudioPlayer countdownMusic;
    [SerializeField] AudioPlayer gameOverMusic;
    [SerializeField] AudioPlayer ambientMusic;

    [Header("SFX")]
    [SerializeField] AudioPlayer fireSFX;
    [SerializeField] AudioPlayer wheelSFX;
    [SerializeField] AudioPlayer uiButtonSFX;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        LoadPlayerPrefs();
        menuBackground.Play();
    }

    void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("MasterVol")) masterVolume = PlayerPrefs.GetFloat("MasterVol");
        if (PlayerPrefs.HasKey("MusicVol")) musicVolume = PlayerPrefs.GetFloat("MusicVol");
        if (PlayerPrefs.HasKey("SFXVol")) sfxVolume = PlayerPrefs.GetFloat("SFXVol");
        if (PlayerPrefs.HasKey("Mute")) mute = PlayerPrefs.GetInt("Mute") == 1? true: false;

        UpdateVolumes();
        UpdateUI();
    }

    void SavePlayerPrefs()
    {
        Debug.Log("Saved Player Audio Preferences");
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
        PlayerPrefs.SetInt("Mute", mute ? 1 : 0);
    }


    void UpdateVolumes()
    {
        foreach(AudioPlayer s in MusicSources())
        {
            s.SetVolume(MusicVolume * MasterVolume);
            s.SetMute(mute);
        }

        foreach(AudioPlayer s in SFXSources())
        {
            s.SetVolume(SFXVolume * MasterVolume);
            s.SetMute(mute);
        }

        OnAudioSettingsUpdate?.Invoke(this, new AudioEventArgs());
        SavePlayerPrefs();
    }

    void UpdateUI()
    {
        masterSlider.value = masterVolume;
        pauseMasterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        pauseMusicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        pauseSfxSlider.value = sfxVolume;
        muteToggle.isOn = mute;
        pauseMuteToggle.isOn = mute;
    }

    
    void OnValuesChange()
    {
        UpdateVolumes();
        UpdateUI();
    }

    AudioPlayer[] MusicSources()
    {
        return new AudioPlayer[]
        {
            menuBackground,
            gameBackground,
            countdownMusic, 
            gameOverMusic,
            ambientMusic
        };
    }

    AudioPlayer[] SFXSources()
    {
        return new AudioPlayer[]
        {
            fireSFX,
            wheelSFX,
            uiButtonSFX
        };
    }

    public void SwitchToMenuMusic()
    {
        menuBackground.Play();
        gameBackground.Stop();
        gameOverMusic.Stop();
        countdownMusic.Stop();
        ambientMusic.Stop();
    }

    public void SwitchToGameMusic()
    {
        menuBackground.Stop();
        gameBackground.Play();
        ambientMusic.Play();
    }

    public void SwitchToGameOverMusic()
    {
        menuBackground.Stop();
        gameBackground.Stop();
        gameOverMusic.Play();
        countdownMusic.Stop();
        ambientMusic.Play();
    }

    public void PlayFireSFX()
    {
        fireSFX.Play();
    }

    public void PlayCountdownMusic()
    {
        countdownMusic.Play();
    }

    public void StopCountdownMusic()
    {
        countdownMusic.Stop();
    }

    public void PlayUIButtonSFX()
    {
        uiButtonSFX.Play();
    }
}

public class AudioEventArgs : EventArgs
{
}

