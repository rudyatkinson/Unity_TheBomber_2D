using UnityEngine;

[System.Obsolete]
public class PlayerPrefs : MonoBehaviour
{
    public static PlayerPrefs _Instance { get; private set; }

    public string playerSkinName { get; private set; }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        else
            _Instance = this;
    }

    private void Start()
    {
        GEvents.PlayerSkinChanged.AddListener(OnPlayerSkinChanged);
    }

    private void OnPlayerSkinChanged(string skinName) => playerSkinName = skinName;

    private void OnDestroy()
    {
        GEvents.PlayerSkinChanged.RemoveListener(OnPlayerSkinChanged);
    }
}
