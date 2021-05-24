using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public GameObject gameOverPanel;

    #region Components

    private PlayerController thePlaneController;
    private PlayerCollisions playerCollisions;
    private Animator animator;
    private Rigidbody2D rb2d;
    private Collider2D collider2d;
    private AudioSource source;

    #endregion
    #region VFXs

    public GameObject explosionVFX;
    public GameObject fireVFX;

    #endregion
    #region CooldownSettings

    private float explosionCd = 0.25f;
    private float explosionCdOffset = 0.1f;

    #endregion

    private void Start()
    {
        thePlaneController = PlayerComponents._Instance.ThePlaneController;
        playerCollisions = PlayerComponents._Instance.PlayerCollisions;
        animator = PlayerComponents._Instance.Animator;
        rb2d = PlayerComponents._Instance.Rb2D;
        collider2d = PlayerComponents._Instance.Collider2D;
        source = PlayerComponents._Instance.AudioSource;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        // Different codes work according to the detected tag.
        switch (tag)
        {
            case "Ground":
            case "!Ammunition":
            case "!Tank":
                DestroyPlane();
                StartCoroutine(Explosions());
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // Different codes work according to the detected tag.
        switch (tag)
        {
            case "!Plane":
                DestroyPlane();
                StartCoroutine(Explosions());

                rb2d.AddForceAtPosition(transform.right * 10, 
                    new Vector2(1 - (collider2d.bounds.size.x / 2), 0));
                break;
            case "PlaneProjectile":
            case "TankProjectile":
                if(SoundManager.IsVibrationOn) Handheld.Vibrate();
                GEvents.PlayerDamageReceived?.Invoke(1);
                animator.SetTrigger("DamageReceived");
                PlayerComponents._Instance.PlayerDamageHandler.enabled = true;
                break;
        }
    }

    #region Coroutines

    private void DestroyPlane()
    {
        GEvents.GameOver.Invoke();

        // Deactivating the controls of player and set the animator speed to 0.
        thePlaneController.enabled = false;
        playerCollisions.enabled = false;
        animator.speed = 0f;
        source.volume = 0f;
        rb2d.constraints = RigidbodyConstraints2D.None;
        transform.GetChild(1).gameObject.SetActive(false);

        // Deactivating movement of the main camera.
        Camera.main.gameObject.GetComponent<CameraMovement>().enabled = false;

        gameOverPanel.SetActive(true);
    }

    IEnumerator Explosions()
    {
        while (true)
        {
            // Get bound size and bound center point to use later.
            var center = collider2d.bounds.center;
            var sizeX = collider2d.bounds.size.x;
            var sizeY = collider2d.bounds.size.y;

            // Find min and max point of x and y coordinates. 
            var minX = center.x - (sizeX / 2);
            var maxX = center.x + (sizeX / 2);
            var minY = center.y - (sizeY / 2);
            var maxY = center.y + (sizeY / 2);

            // Get a random point via min and max points to instantiate vfx at random point.
            var rndX = Random.Range(minX, maxX);
            var rndY = Random.Range(minY, maxY);
            var explosionPos = new Vector2(rndX, rndY);

            // Set a rotation to use later to instantiate vfx with true rotation.
            var rot = Quaternion.Euler(0, 0, 0);

            // Instantiating the explosionVFX
            GameObject explosion = Instantiate(explosionVFX,
                explosionPos,
                rot, 
                null);

            // Setting cooldown for wait to next explosion.
            var minSec = Random.Range(explosionCd - explosionCdOffset, 
                explosionCd + explosionCdOffset);
            yield return new WaitForSeconds(minSec);
        }
    }

    #endregion
}
