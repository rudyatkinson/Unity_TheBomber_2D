using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    public static Notification _Instance { get; private set; }

    #region Prefixes
    public GameObject playerNotificationPrefab;
    public Transform playerNotificaitonTransform;
    #endregion
    #region Enemy Sprites For Notification
    public Sprite artillerySprite;
    public Sprite planeSprite;
    public Sprite ammunitionSprite;
    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
            Destroy(gameObject);
        else
        {
            _Instance = this;
        }

        GEvents.NotifyPlayer.AddListener(OnPlayerNotified);
        GEvents.GameOver.AddListener(DisableNotifications);
    }

    /// <summary>
    /// Instantiates notificates which comes from events
    /// </summary>
    /// <param name="message">Message text</param>
    /// <param name="messageSprite">Message icon, sprite</param>
    private void OnPlayerNotified(string message, Sprite messageSprite)
    {
        var go = Instantiate(playerNotificationPrefab,
            playerNotificaitonTransform);

        if (messageSprite != null)
            go.transform.GetChild(0).GetComponent<Image>().sprite = messageSprite;
        else
            go.transform.GetChild(0).gameObject.SetActive(false);

        go.transform.GetChild(1).GetComponent<TMP_Text>().text = message;
    }

    private void DisableNotifications()
    {
        playerNotificaitonTransform.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GEvents.NotifyPlayer.RemoveListener(OnPlayerNotified);
        GEvents.GameOver.RemoveListener(DisableNotifications);
    }
}
