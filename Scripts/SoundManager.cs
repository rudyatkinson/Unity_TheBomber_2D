using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//RUDY: OnValueChanged eventleri yazýlacak.

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Instance { get; private set; }

    public AudioSource source { get; private set; }

    #region UI Elements
    public Slider audioVolumeSlider;
    public Toggle audioMuteToggle;
    public Toggle vibrationToggle;
    #endregion

    #region AudioClips
    public AudioClip artillerFireAudio;
    public AudioClip explosionAudio;
    #endregion

    // Max volume setting for each kind of voices
    private float maxVolume = 0.15f;
    // Contains info that audio sources gonna play or not
    public static bool PlayAudioSetting { get; private set; }
    // Contains info that vibration is on or not
    public static bool IsVibrationOn { get; private set; }
    // Contains audio volume slider value
    [Range(0f, 1f)]
    public static float PlayAudioVolume;

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
            Destroy(gameObject);
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this);
        }

        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadSettings();

        // Actions added to audio settings.
        audioVolumeSlider.onValueChanged.AddListener(OnValueChangedVolumeSlider);
        audioMuteToggle.onValueChanged.AddListener(OnValueChangedMuteToggle);
        vibrationToggle.onValueChanged.AddListener(OnValueChangedVibrationToggle);

        GEvents.BackToMainMenu.AddListener(DestroyManager);
    }

    /// <summary>
    /// <see cref="UnityEngine.PlayerPrefs"/>
    /// loads setting if already has the key, otherwise sets the volume as default.
    /// </summary>
    private void LoadSettings()
    {
        if (UnityEngine.PlayerPrefs.HasKey("audioVolume"))
        {
            PlayAudioVolume = UnityEngine.PlayerPrefs.GetFloat("audioVolume", audioVolumeSlider.value);
            audioVolumeSlider.value = PlayAudioVolume;
        }

        if (UnityEngine.PlayerPrefs.HasKey("vibrationSetting"))
        {
            IsVibrationOn = System.Convert.ToBoolean(UnityEngine.PlayerPrefs.GetInt("vibrationSetting", 1));
            vibrationToggle.isOn = IsVibrationOn;
        }

        if (UnityEngine.PlayerPrefs.HasKey("muteSetting"))
        {
            PlayAudioSetting = System.Convert.ToBoolean(UnityEngine.PlayerPrefs.GetInt("muteSetting", 1));
            audioMuteToggle.isOn = !PlayAudioSetting;
        }

        source.volume = PlayAudioVolume * maxVolume;
        source.mute = PlayAudioSetting;
    }

    private void OnValueChangedVolumeSlider(float volume)
    {
        PlayAudioVolume = audioVolumeSlider.value;
        source.volume = PlayAudioVolume * maxVolume;

        //Informs registered sources if not null.
        GEvents.AudioVolumeChanged?.Invoke(PlayAudioVolume);
        //Sets a float to contain audio volume.
        UnityEngine.PlayerPrefs.SetFloat("audioVolume", PlayAudioVolume);
    }

    private void OnValueChangedMuteToggle(bool isMuted)
    {
        PlayAudioSetting = !isMuted;
        source.mute = isMuted;

        //Informs registered sources if not null.
        GEvents.AudioMuteSettingChanged?.Invoke(isMuted);
        //Sets an int to contain mute setting.
        UnityEngine.PlayerPrefs.SetInt("muteSetting", System.Convert.ToInt32(!isMuted));
    }

    private void OnValueChangedVibrationToggle(bool isVibrate)
    {
        IsVibrationOn = isVibrate;
        //Sets an int to contain vibration setting.
        UnityEngine.PlayerPrefs.SetInt("vibrationSetting", System.Convert.ToInt32(IsVibrationOn));
    }

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
        // Plays clip in an instantiated object with static method.
        if (PlayAudioSetting)
            AudioSource.PlayClipAtPoint(clip, position, PlayAudioVolume);
    }

    /// <summary>
    /// Destroys the object and set singleton free before main menu loaded.
    /// </summary>
    private void DestroyManager()
    {
        _Instance = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GEvents.BackToMainMenu.RemoveListener(DestroyManager);
        // PlayerPrefs saves audio volume before object destroyed.
        UnityEngine.PlayerPrefs.Save();
    }
}
