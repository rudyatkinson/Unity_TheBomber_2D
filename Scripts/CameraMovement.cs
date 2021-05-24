using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speed;

    private void Awake()
    {
        speed = 2f;
    }
    
    void FixedUpdate()
    {
        transform.position += new Vector3(speed, 0, 0) * Time.fixedDeltaTime;
    }
}
