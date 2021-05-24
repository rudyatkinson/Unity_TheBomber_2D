using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetDisplay : MonoBehaviour
{
    //Target image
    public Image loadImage;

    #region Settings
    float time = 0;
    float duration = 0;
    float percent = 0;
    #endregion

    private void FixedUpdate()
    {
        // if duration not set yet returns
        if (duration == 0) return;

        // calculating elapsed time percentage, and given the fillAmount value of loadImage
        time += Time.fixedDeltaTime;
        percent = time / duration;
        loadImage.fillAmount = percent;

        // Script is considered complete and disabled when percentage comes to 100
        if (percent >= 1)
        {
            gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void OnDisable()
    {
        // All property values set back as 0 when script disabled.
        time = 0;
        duration = 0;
        percent = 0;

        loadImage.fillAmount = 0;
    }

    /// <summary>
    /// Used to set duration time.
    /// </summary>
    public void SetTargetDisplay(float duration) => this.duration = duration;


}
