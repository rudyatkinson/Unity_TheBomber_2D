using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    private float horizontalSpeed;
    private float fireRateAsSec;
    #endregion

    #region Prefabs
    public GameObject collisionVFX;
    public GameObject collisionExplosion;
    public GameObject projectile;
    #endregion

    public AudioSource source;
    private float maxVolume = 0.075f;
    private bool isAvailable = true;

    private void Start()
    {
        horizontalSpeed = 2f;
        fireRateAsSec = Random.Range(2f, 3f);

        StartCoroutine("Fire");

        // source plays audio if global audio not muted.
        if (SoundManager.PlayAudioSetting)
        {
            OnVolumeChanged(SoundManager.PlayAudioVolume);
            source.Play();
        }


        // Registered to the audioVolumeChanged to get inform when global volume setting is changed.
        GEvents.AudioVolumeChanged.AddListener(OnVolumeChanged);
        // Registered to the audioMuteSettingChanged to get inform when global volume muted or unmuted.
        GEvents.AudioMuteSettingChanged.AddListener(OnMuteSettingChanged);
    }

    private void FixedUpdate()
    {
        // Basic Movement
        var movement = new Vector3(horizontalSpeed * Time.fixedDeltaTime, 0, 0);
        transform.position -= movement;

        // Deactivates fire if player behind of the enemy
        if (isAvailable &&
            Camera.main.transform.position.x - 2 > transform.position.x)
        {
            StopCoroutine("Fire");
            isAvailable = false;
        }
    }

    IEnumerator Fire()
    {
        while(true)
        {
            yield return new WaitForSeconds(fireRateAsSec);

            //Instantiating the bullet and setting transform.
            GameObject go = Instantiate(projectile, transform);
            go.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Instantiating both of the VFXs and setting position to planes position.
            GameObject goExplosion = Instantiate(collisionExplosion, null);
            goExplosion.transform.position = transform.position;

            GameObject goVFX = Instantiate(collisionVFX, null);
            goVFX.transform.position = transform.position;
        }
        
        Destroy(gameObject);
    }

    /// <summary>
    /// This method using to get inform via event.
    /// </summary>
    private void OnVolumeChanged(float volume)
    {
        float tempVol = maxVolume * volume;
        source.volume = tempVol;
    }

    private void OnMuteSettingChanged(bool isMuted)
    {
        source.mute = isMuted;
    }

    private void OnDestroy()
    {
        // Method removing from the event to avoid exceptions.
        GEvents.AudioVolumeChanged.RemoveListener(OnVolumeChanged);
        GEvents.AudioMuteSettingChanged.RemoveListener(OnMuteSettingChanged);
    }
}
