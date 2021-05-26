using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings _Instance { get; private set; }


    #region UI Elements
    public Slider audioVolumeSlider;
    public Toggle audioMuteToggle;
    public Toggle vibrationToggle;
    #endregion

    #region Setting Variables
    // Contains audio volume
    [Range(0f, 1f)]
    public static float PlayAudioVolume; { get; private set; }
    // Contains info that audio sources gonna play or not
    public static bool PlayAudioSetting { get; private set; }
    // Contains info that vibration is on or not
    public static bool IsVibrationOn { get; private set; }
    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
            Destroy(gameObject);
        else
        {
            _Instance = this;
        }
    }

    private void Start()
    {
        LoadSettings();

        // Actions added to audio settings.
        audioVolumeSlider.onValueChanged.AddListener(OnValueChangedVolumeSlider);
        audioMuteToggle.onValueChanged.AddListener(OnValueChangedMuteToggle);
        vibrationToggle.onValueChanged.AddListener(OnValueChangedVibrationToggle);
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
            IsVibrationOn = (UnityEngine.PlayerPrefs.GetInt("vibrationSetting", 1) == 1 ? true : false);
            vibrationToggle.isOn = IsVibrationOn;
        }

        if (UnityEngine.PlayerPrefs.HasKey("muteSetting"))
        {
            PlayAudioSetting = (UnityEngine.PlayerPrefs.GetInt("muteSetting", 1)  == 1 ? true : false);
            audioMuteToggle.isOn = !PlayAudioSetting;
        }
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
        UnityEngine.PlayerPrefs.SetInt("muteSetting", isMuted ? 1 : 0);
    }

    private void OnValueChangedVibrationToggle(bool isVibrate)
    {
        IsVibrationOn = isVibrate;
        //Sets an int to contain vibration setting.
        UnityEngine.PlayerPrefs.SetInt("vibrationSetting", IsVibrationOn  ? 1 : 0);
    }

    private void OnDestroy()
    {
        UnityEngine.PlayerPrefs.Save();
    }
}
