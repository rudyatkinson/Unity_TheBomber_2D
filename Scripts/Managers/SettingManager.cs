using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager _Instance { get; private set; }

    // Contains info that vibration is on or not
    public static bool _VibrationEnabled { get; private set; }
    // Contains audio volume slider value
    public static float _PlayAudioVolume { get; private set; }

    private float defaultAudioVolume = 0.8f;

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
            Destroy(gameObject);
        else
        {
            _Instance = this;
        }

        GEvents.AudioVolumeChanged.AddListener(OnAudioVolumeChanged);
        GEvents.VibrationSettingChanged.AddListener(OnVibrationSettingChanged);

        LoadAudioVolumeSetting();
        LoadVibrationSetting();
    }

    /// <summary>
    /// Loads audio volume setting if it is exist or set it with default value and triggers AudioVolumeChanged event. 
    /// </summary>
    private void LoadAudioVolumeSetting()
    {
        var result = UnityEngine.PlayerPrefs.GetFloat("audioVolume", defaultAudioVolume);
        GEvents.AudioVolumeChanged?.Invoke(result);
    }

    /// <summary>
    /// Loads vibration setting if it is exist or set it with default value.
    /// </summary>
    private void LoadVibrationSetting()
    {
        var result = UnityEngine.PlayerPrefs.GetInt("vibrationSetting", 1) == 1 ? true : false;
        GEvents.VibrationSettingChanged?.Invoke(result);
    }

    private void OnAudioVolumeChanged(float audioVolume)
    {
        _PlayAudioVolume = audioVolume;
        UnityEngine.PlayerPrefs.SetFloat("audioVolume", audioVolume);
    }

    private void OnVibrationSettingChanged(bool vibrateEnabled)
    {
        _VibrationEnabled = vibrateEnabled;
        UnityEngine.PlayerPrefs.SetInt("vibrationSetting", vibrateEnabled ? 1 : 0);
    }

    private void OnDestroy()
    {
        GEvents.AudioVolumeChanged.RemoveListener(OnAudioVolumeChanged);
        GEvents.VibrationSettingChanged.RemoveListener(OnVibrationSettingChanged);
        UnityEngine.PlayerPrefs.Save();
    }
}
