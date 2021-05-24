using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModuleDetectFront : MonoBehaviour
{
    bool isBuildable = true;
    float cooldown = 0.1f;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Instantiates next module
        if(isBuildable)
        {
            GEvents.BuildNextGeneration.Invoke();
            isBuildable = false;
            StartCoroutine(Cooldown());
        }
        
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        isBuildable = true;
    }
}
