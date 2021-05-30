using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAttack : MonoBehaviour
{
    #region Bomb Variables
    public int maxBombCount { get; private set; } = 5;
    public int bombCount { get; private set; } = 5;
    #endregion
    #region Time Variables
    public float bombRefillTime { get; private set; } = 5f;
    private float expiredTime = 0f;
    #endregion

    public GameObject bombPrefab;
    public Sprite bombSprite;
    public Transform bombEjectTransform;

    public BombUI bombUI;

    public void DropBomb()
    {
        // Instantiates bomb at prefixed position and triggers BombDropped event if player have bomb
        if(bombCount > 0)
        {
            GameObject bomb = Instantiate(bombPrefab, bombEjectTransform.position, Quaternion.identity, null);
            bomb.transform.position = bombEjectTransform.position;

            GEvents.BombDropped?.Invoke(--bombCount);
        }
    }

    private void FixedUpdate()
    {
        // Bomb starts to refill if bomb count less than max size
        if(bombCount < maxBombCount)
        {
            expiredTime += Time.fixedDeltaTime;
            // Informs bombRefilled event 
            GEvents.BombRefilled.Invoke(bombCount, expiredTime / bombRefillTime);
            // Resets variables if expiredTimes is done
            if (expiredTime >= bombRefillTime)
            {
                bombCount++;
                expiredTime = 0f;
                // Sends notify
                GEvents.NotifyPlayer.Invoke($"Refilled {bombCount}/{maxBombCount}", bombSprite);
            }
        }
    }
}
