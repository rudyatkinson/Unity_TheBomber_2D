using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModuleDetectBack : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
