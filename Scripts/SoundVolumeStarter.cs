using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeStarter : MonoBehaviour
{
    public float maxVolume;

    public AudioSource source;

    private void OnEnable()
    {
        // adjusts max volume of audioSource which attached in object
        if (SoundManager.PlayAudioSetting)
        {
            var volume = maxVolume * SoundManager.PlayAudioVolume;
            source.volume = volume;
        }
        else source.Stop();
    }
}
