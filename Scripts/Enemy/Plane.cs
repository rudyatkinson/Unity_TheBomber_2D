using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Destroyable
{
    #region Variables
    #region HorizontalSpeed
    private float horizontalSpeed;
    private float minHorizontalSpeed = 2f;
    private float maxHorizontalSpeed = 3f;
    #endregion
    #region FireRate
    private float fireRateAsSec;
    private float minFireRateAsSec = 1.75f;
    private float maxFireRateAsSec = 2.5f;
    #endregion
    private float maxVolume = 0.075f;
    #region Explosion
    private float explosionCd = 0.50f;
    private float explosionCdOffset = 0.075f;
    #endregion
    #region After Collision
    private float gravityValue = 0.5f;
    private float pushPower = 20f;
    #endregion
    #endregion

    #region Prefabs
    public GameObject collisionVFX;
    public GameObject collisionExplosion;
    public GameObject projectile;
    #endregion

    private AudioSource source;
    private Collider2D collider2d;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        horizontalSpeed = Random.Range(minHorizontalSpeed, maxHorizontalSpeed);
        fireRateAsSec = Random.Range(minFireRateAsSec, maxFireRateAsSec);

        StartCoroutine("Fire");

        // source plays audio if global audio not muted.
        OnVolumeChanged(SettingManager._PlayAudioVolume);
        source.Play();

        // Registered to the audioVolumeChanged to get inform when global volume setting is changed.
        GEvents.AudioVolumeChanged.AddListener(OnVolumeChanged);
    }

    private void FixedUpdate()
    {
        // Basic Movement
        var movement = new Vector3(horizontalSpeed * Time.fixedDeltaTime, 0, 0);
        transform.position -= movement;

        // Deactivates fire if player behind of the enemy
        if (Camera.main.transform.position.x - 2 > transform.position.x)
        {
            StopCoroutine("Fire");
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
        switch(collision.tag)
        {
            case "Player":
            case "Bomb":
                Destroyed();
                Disable();
                break;
        }
    }

    /// <summary>
    /// This method using to get inform via event.
    /// </summary>
    private void OnVolumeChanged(float volume)
    {
        float tempVol = maxVolume * volume;
        source.volume = tempVol;
    }

    protected override void Destroyed()
    {
        //Instantiating both of the VFXs and setting position to planes position.
        InstantiateExplosive(collisionVFX, 
            explosionCd, 
            explosionCdOffset, 
            collider2d, 
            transform);
    }

    protected override void Disable()
    {
        // Gravity scale changed after collison to drop effect.
        rb2d.gravityScale = gravityValue;
        // Trigger property of collider disabled to collide with ground.
        collider2d.isTrigger = false;
        // Push the enemy plane forward to give drop effect.
        rb2d.AddForce(Vector2.left * pushPower);

        // Functionalities disabled.
        enabled = false;
    }

    private void OnDestroy()
    {
        // Method removing from the event to avoid exceptions.
        GEvents.AudioVolumeChanged.RemoveListener(OnVolumeChanged);
    }
}
