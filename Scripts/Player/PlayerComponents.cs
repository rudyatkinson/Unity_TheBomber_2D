using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class contains player components. Reachable via singleton.
/// </summary>
public class PlayerComponents : MonoBehaviour
{
    public static PlayerComponents _Instance { get; private set; }

    #region Components Properties
    
    public Transform PlayerTransform { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }
    public Collider2D Collider2D { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerController ThePlaneController { get; private set; }
    public PlayerCollisions PlayerCollisions { get; private set; }
    public PlayerDamageHandler PlayerDamageHandler { get; private set; }
    public AudioSource AudioSource { get; private set; }

    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this) 
            Destroy(gameObject);
        else 
            _Instance = this;

        #region Component Properties Declaring

        PlayerTransform = GetComponent<Transform>();
        Rb2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
        ThePlaneController = GetComponent<PlayerController>();
        PlayerCollisions = GetComponent<PlayerCollisions>();
        PlayerDamageHandler = GetComponent<PlayerDamageHandler>();
        AudioSource = GetComponent<AudioSource>();

        #endregion
    }
}
