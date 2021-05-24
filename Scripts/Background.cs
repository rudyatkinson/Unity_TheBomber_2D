using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    #region Variables
    private float length;
    private float startpos;
    public float firstSpeed;
    #endregion
    private Transform cam;
    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startpos = transform.position.x;
        length = render.bounds.size.x;

        cam = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        // Refreshes background image when it go out
        float temp = (cam.transform.position.x * (1 - firstSpeed));
        float dist = (cam.position.x * firstSpeed);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
