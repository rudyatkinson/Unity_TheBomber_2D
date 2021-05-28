using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region UI Elements
    public Slider audioVolumeSlider;
    public Toggle vibrationToggle;
    #endregion

    private void Start()
    {
        // Actions added to audio settings.
        audioVolumeSlider.onValueChanged.AddListener(OnAudioVolumeSliderChanged);
        vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);
    }

    private void OnEnable()
    {
        audioVolumeSlider.value = SettingManager._PlayAudioVolume;
        vibrationToggle.isOn = SettingManager._VibrationEnabled;
    }

    private void OnAudioVolumeSliderChanged(float volume)
    {
        //Informs registered sources if not null.
        GEvents.AudioVolumeChanged?.Invoke(volume);        
    }

    private void OnVibrationToggleChanged(bool vibrationEnabled)
    {
        //Informs registered setting manager if not null.
        GEvents.VibrationSettingChanged.Invoke(vibrationEnabled);
    }
}
