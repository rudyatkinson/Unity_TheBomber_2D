using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    #region Variables
    private float speed;
    private float explosionOffset;
    #endregion
    #region Prefabs
    public GameObject explosionVFX;
    public GameObject explosionPS;
    #endregion

    private void Awake()
    {
        speed = 5f;
        explosionOffset = Random.Range(0f, 1f);
    }

    private void Update()
    {
        //Movement handled with MoveTowards.
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);   
    }

    private void FixedUpdate()
    {
        //Distance calculated to instantiate explosive VFX and destroy this bullet.
        if (Vector2.Distance(transform.position, target.position) <= explosionOffset)
        {
            DestroyBullet();
        }
    }

    public void LookAt(Transform target)
    {
        this.target = target;

        //Calculating angle and rotating with Quaternion.
        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyBullet();
    }

    /// <summary>
    /// Instantiates VFX and destroy object
    /// </summary>
    private void DestroyBullet()
    {
        GameObject go = Instantiate(explosionVFX, null);
        go.transform.position = transform.position;
        GameObject ps = Instantiate(explosionPS, null);
        ps.transform.position = transform.position;
        Destroy(gameObject);
    }
}
