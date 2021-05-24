using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Destroyable
{
    #region VFX Prefabs
    public GameObject explosionVFX;
    public GameObject fireVFX;
    #endregion
    #region Components
    private Collider2D collider2d;
    private BulletSpawner bulletSpawner;
    #endregion
    #region Variables
    private int fireCount = 2;
    private float explosionCd = 0.40f;
    private float explosionCdOffset = 0.075f;
    private bool isAvailable = true;
    #endregion

    private void Awake()
    {
        bulletSpawner = GetComponent<BulletSpawner>();
        collider2d = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if(isAvailable &&
            Camera.main.transform.position.x - 2 > transform.position.x)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Instantiates VFX objects if collided with player or bomb.
        switch (collision.transform.tag)
        {
            case "Player":
            case "Bomb":
                StartCoroutine(InstantiateExplosive(explosionVFX, 
                    explosionCd, 
                    explosionCdOffset, 
                    collider2d, 
                    transform));

                InstantiateFire(fireVFX,
                    fireCount, 
                    collider2d, 
                    transform);

                Deactivate();
                tag = "Destroyed";
                break;
        }
    }

    /// <summary>
    /// Triggers base deactivate method and disable bullet spawner
    /// </summary>
    protected override void Deactivate()
    {
        // Sets bulletSpawners enabled property as false to make sure doesnt fire anymore.
        bulletSpawner.enabled = false;
        // Sets false because do not want to check position of player anymore.
        isAvailable = false;
        // Target Display disabled
        bulletSpawner.targetTransform.GetComponent<TargetDisplay>().enabled = false;
        bulletSpawner.targetTransform.gameObject.SetActive(false);

        enabled = false;
    }

    
}
