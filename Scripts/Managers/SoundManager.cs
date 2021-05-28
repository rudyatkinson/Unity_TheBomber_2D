using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Instance { get; private set; }

    public AudioSource source { get; private set; }

    #region AudioClips
    public AudioClip artillerFireAudio;
    public AudioClip explosionAudio;
    #endregion

    // Max volume setting for each kind of voices
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
        GEvents.AudioVolumeChanged.AddListener(OnAudioVolumeChanged);
    }

    /// <summary>
    /// Adjusts source volume which is using to play siren audio.
    /// </summary>
    private void OnAudioVolumeChanged(float volume)
    {
        source.volume = volume * maxVolume;
    }

    /// <summary>
    /// Use to play static method of AudioSource and adjusts volume with <see cref="SettingManager._PlayAudioVolume"/>
    /// </summary>
    /// <param name="clip">Clip which is wanted to play</param>
    /// <param name="position">Position of clip where gonna play at</param>
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, SettingManager._PlayAudioVolume);
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
        GEvents.AudioVolumeChanged.RemoveListener(OnAudioVolumeChanged);
    }
}
