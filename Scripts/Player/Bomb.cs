using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Rigidbody2D rb2d;

    #region Prefabs
    public GameObject explosionVFX;
    public GameObject firePrefab;
    #endregion

    void Update()
    {
        // That rotates the bomb until it look down as smoothly as possible.
        // Velocity affects rotation speed.
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.Euler(0, 0, -90), 
            0.1f * Time.deltaTime * rb2d.velocity.sqrMagnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();

        // Instantiates a fireVFX if collided objects tag is ground,
        // tries to detect enemy and send inform if collided object not ground.
        if(collision.tag == "Ground")
        {
            Instantiate(firePrefab, 
                transform.position, 
                Quaternion.Euler(0, 0, 0), 
                null);
        }
        else if(collision.tag[0] == '!')
        {
            string name = collision.tag.Remove(0, 1);
            // Triggers event to increase player score
            GEvents.EnemyDestroyed.Invoke(name);
            // Picks enemy sprite to send it as notification
            Sprite notificationSprite = null;
            switch(name)
            {
                case "Plane":
                    notificationSprite = Notification._Instance.planeSprite;
                    break;
                case "Tank":
                    notificationSprite = Notification._Instance.artillerySprite;
                    break;
                case "Ammunition":
                    notificationSprite = Notification._Instance.ammunitionSprite;
                    break;                    
            }
            GEvents.NotifyPlayer.Invoke("Destroyed", notificationSprite);
        }
    }

    /// <summary>
    /// Instantiates an explosionVFX before destroyed
    /// </summary>
    void Explode()
    {
        Instantiate(explosionVFX, 
            transform.position, 
            transform.rotation);

        Destroy(gameObject);
    }
    
}
