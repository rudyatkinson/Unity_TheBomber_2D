using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    private void Awake()
    {
        // Script disables itself after PlayerComponents set it as a component.
        enabled = false;
    }

    private void OnEnable()
    {
        // Player control disabled for a sec.
        PlayerComponents._Instance.Animator.speed = 1;
        PlayerComponents._Instance.AudioSource.mute = true;
        PlayerComponents._Instance.ThePlaneController.enabled = false;
    }

    public void DisableDamageHandler() => enabled = false;

    private void OnDisable()
    {
        // Player control enabled after animation.
        PlayerComponents._Instance.AudioSource.mute = false;
        PlayerComponents._Instance.ThePlaneController.enabled = true;
    }
}
