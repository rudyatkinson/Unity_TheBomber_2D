using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotification : MonoBehaviour
{
    public float destroyAfterSeconds;

    private void Awake()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);
        GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
