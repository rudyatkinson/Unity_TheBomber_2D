using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    #region Target
    public Transform[] targetTransforms;
    public Transform targetTransform;
    #endregion
    public Transform projectileSpawnTransform;

    private Animator anim;

    public float spawnCooldown;
    public float spawnCooldownOffset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine("SpawnLandBullet");
    }

    IEnumerator SpawnLandBullet()
    {
        float duration;

        while (true)
        {
            // Picks a random number to wait next attack
            duration = Random.Range(spawnCooldown - spawnCooldownOffset, spawnCooldown);

            // Picks a random target position and set it visible
            targetTransform = targetTransforms[Random.Range(0, targetTransforms.Length)];
            targetTransform.gameObject.SetActive(true);
            var targetDisplay = targetTransform.GetComponent<TargetDisplay>();
            targetDisplay.enabled = true;
            targetDisplay.SetTargetDisplay(duration);

            yield return new WaitForSeconds(duration);
            // animation triggers fire method
            anim.SetTrigger("Fire");
            yield return new WaitForSeconds(spawnCooldownOffset);
        }
    }

    public void Fire()
    {
        //Fire audio plays at the point when artillery fire via static PlayClipAtPoint.
        SoundManager.PlayClipAtPoint(SoundManager._Instance.artillerFireAudio,
            transform.position);

        //Instantiating the bullet and caching component to use it next time.
        GameObject go = Instantiate(bulletPrefab, transform);
        go.transform.position = projectileSpawnTransform.position;

        //Setting transform and some calculations for rotation to target via LookAt method which in EnemyBullet.cs
        EnemyBullet goEB = go.GetComponent<EnemyBullet>();
        goEB.LookAt(targetTransform);
    }

    private void OnDisable()
    {
        StopCoroutine("SpawnLandBullet");
    }
}
