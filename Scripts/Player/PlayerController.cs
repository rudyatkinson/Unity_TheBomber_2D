using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Please add unique layer called ground for layermask and select it in the inspector before running this script!
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask ceilLayerMask;

    //Vector2 movement;
    private float maxVelocity = 3f;
    private float minVelocity = -2f;

    //Fuel disabled
    //private float maxFuel = 100;
    //private float pushConsumption = 0;
    //private float currentFuel;
    //private float consumptionRate; //consumption rate multiplies with distance range to creates a decrease rate of the fuel(UI)

    // Max volume setting for background audio which is siren.
    private float maxVolume = 0.10f;

    //Plane rotation values to give true rotation when push the plane up.
    private Quaternion planeRotation;

    #region Components
    private Rigidbody2D rb2d;
    private Collider2D collider2d;
    private Animator animator;
    private AudioSource source;
    #endregion

    //fuel bar UI from created Canvas
    //public FuelBar fuelBar;

    // Start is called before the first frame update
    void Start()
    {
#region Component Declaring

        rb2d = PlayerComponents._Instance.Rb2D;
        collider2d = PlayerComponents._Instance.Collider2D;
        animator = PlayerComponents._Instance.Animator;
        source = PlayerComponents._Instance.AudioSource;

#endregion

        planeRotation = Quaternion.Euler(0, 0, -10);

        // AudioSource which had in SoundManager plays siren audio after a sec.
        SoundManager._Instance.source.PlayDelayed(1f);
        // Start Notification
        GEvents.NotifyPlayer.Invoke("Good Luck!", null);

        // Adjusts mute property of audio Source false or true according to setting.
        source.mute = !SoundManager.PlayAudioSetting;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2d.velocity.y > maxVelocity) rb2d.velocity = new Vector2(0, maxVelocity);
        else if (rb2d.velocity.y < minVelocity) rb2d.velocity = new Vector2(0, minVelocity);
    }

    // Adjusting according to velocity
    private void FixedUpdate()
    {
        if (rb2d.velocity.y > 0)
        {
            // plane is pointing up,
            planeRotation = Quaternion.Euler(0, 0, 10);

            // animator speed changing according to velocity,
            animator.speed = rb2d.velocity.y;

            // audio volume is proportional to velocity and plays if has velocity.
            var rate = rb2d.velocity.y / maxVelocity;
            var volume = rate * maxVolume;
            source.volume = volume;

            if (!source.isPlaying) source.Play();
        }
        else
        {
            // plane is pointing down,
            planeRotation = Quaternion.Euler(0, 0, -10);

            // animator speed and audio volume changes as 0, and audio source stops.
            animator.speed = 0;
            source.volume = 0;

            if (source.isPlaying) source.Stop();
        }

        // plane rotates in the direction it should face.
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            planeRotation,
            0.25f * Time.fixedDeltaTime * rb2d.velocity.sqrMagnitude);
    }

    public void Jump()
    {
        //Distance between player and ceil collider
        float distance = 0.0f;

        //cast a raycast straight up from player
        RaycastHit2D hit = Physics2D.Raycast(collider2d.bounds.center, Vector2.up, Mathf.Infinity, ceilLayerMask);

        //check if ray collides with smth.
        if (hit.collider != null)
        {
            //calculate the distance between ray origin and the surface
            distance = Mathf.Abs(hit.point.y - transform.position.y);
        }

        if (distance > 0.75f && 
            !PlayerComponents._Instance.PlayerDamageHandler.enabled)
        {
            rb2d.AddForce(transform.up * 200f, ForceMode2D.Force);
        }
    }
}
