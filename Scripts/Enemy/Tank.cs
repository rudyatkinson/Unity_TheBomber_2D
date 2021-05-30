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
        // Checks the position of main cam to disable functionalities. 
        if(isAvailable &&
            Camera.main.transform.position.x - 2 > transform.position.x)
        {
            Disable();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroyed();
        Disable();
    }

    /// <summary>
    /// Instantiates VFXs and deactivates all functionalities via Deactivate
    /// </summary>
    protected override void Destroyed()
    {
        // Instantiates VFX objects if collided with player or bomb.
        StartCoroutine(InstantiateExplosive(explosionVFX,
                    explosionCd,
                    explosionCdOffset,
                    collider2d,
                    transform));

        InstantiateFire(fireVFX,
            fireCount,
            collider2d,
            transform);

        // Changes tag to prevent to get score more than once
        tag = "Destroyed";
    }

    /// <summary>
    /// Deactivates functionalities of the enemy if needed.
    /// </summary>
    protected override void Disable()
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
