using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : Destroyable
{
    #region VFX Prefabs
    public GameObject explosionVFX;
    public GameObject fireVFX;
    #endregion
    #region Components
    private Collider2D collider2d;
    private Animator animator;
    #endregion
    #region Variables
    private int fireCount = 5;
    private float explosionCd = 0.40f;
    private float explosionCdOffset = 0.075f;
    #endregion

    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Instantiates VFX if collided object is player or bomb.
        switch (collision.transform.tag)
        {
            case "Player":
            case "Bomb":
                Destroyed();
                Disable();
                break;
        }
    }

    /// <summary>
    /// Instantiates VFX if collided object is player or bomb.
    /// Triggers base deactivate method and set placeholder image invisible
    /// </summary>
    protected override void Destroyed()
    {
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
    /// Disables functionalities of the enemy when get hit.
    /// </summary>
    protected override void Disable()
    {
        animator.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        enabled = false;
    }
}
