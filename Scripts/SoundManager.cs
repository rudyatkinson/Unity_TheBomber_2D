using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Instance { get; private set; }

    public AudioSource source { get; private set; }

    #region AudioClips
    public AudioClip artillerFireAudio;
    public AudioClip explosionAudio;
    #endregion

    // Max volume setting for bgMusicMaxVolume in game
    private float maxVolume = 0.15f;

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
        GEvents.BackToMainMenu.AddListener(DestroyManager);
        
        source.volume = Settings.PlayAudioVolume * maxVolume;
        source.mute = PlayAudioSetting;
    }
    
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
        // Plays clip in an instantiated object with static method.
        if (Settings.PlayAudioSetting)
            AudioSource.PlayClipAtPoint(clip, position, Settings.PlayAudioVolume);
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
    }
}
