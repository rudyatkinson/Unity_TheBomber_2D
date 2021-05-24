using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour
{
    #region Bomb
    public GameObject bombSpritePrefab;
    public Transform bombSpriteTransform;
    #endregion
    #region Player
    private GameObject player;
    private PlaneAttack playerAttack;
    #endregion

    // Includes bomb images to set fill values
    private Image[] instantiatedBombImages;

    // Includes max bomb count to show in the indicator
    int maxBombCount;

    private void Awake()
    {
        // Get required components of player
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<PlaneAttack>();

        // Get maxBombCount for quick reach
        maxBombCount = playerAttack.maxBombCount;

        // Instantiating an array sized with maxBombCount
        instantiatedBombImages = new Image[maxBombCount];

        // Instantiating bomb images in the indicator
        for (int i = 0; i < maxBombCount; i++)
        {
            instantiatedBombImages[i] = Instantiate(bombSpritePrefab,
                bombSpriteTransform).GetComponent<Image>();
        }

        GEvents.BombRefilled.AddListener(OnBombRefilled);
        GEvents.BombDropped.AddListener(OnBombDropped);
    }

    private void OnBombDropped(int bombNumber)
    {
        // Adjusts fillAmount value to 0 after a bomb dropped.
        if(bombNumber + 1 < maxBombCount)
            instantiatedBombImages[bombNumber + 1].fillAmount = 0;
    }

    private void OnBombRefilled(int bombNumber, float fillAmount)
    {
        // Adjusts fillAmount value while refillment.
        instantiatedBombImages[bombNumber].fillAmount = fillAmount;
    }

    private void OnDestroy()
    {
        GEvents.BombRefilled.RemoveListener(OnBombRefilled);
        GEvents.BombDropped.RemoveListener(OnBombDropped);
    }
}