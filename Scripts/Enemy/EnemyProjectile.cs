using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float speed = 8f;
    public float projectileDamage { get; private set; } = 25f;

    public GameObject collisionVFX;

    void FixedUpdate()
    {
        transform.position -= new Vector3(speed * Time.fixedDeltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Instantiates VFX and destroy the object
        if (collision.tag == "Player")
        {
            GameObject go = Instantiate(collisionVFX, null);
            go.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
}
