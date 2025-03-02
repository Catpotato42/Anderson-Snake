using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthFlash : MonoBehaviour
{
    private Color red = new Color(1, 0.24314f, 0.24314f);
    //unused
    private Color green = new Color(0.3546182f, 0.7735849f, 0.3254895f); //this is a REALLY nice green (★‿★)
    //

    public IEnumerator DecreaseHealthFlash () {
        gameObject.SetActive(true);
        Image image = gameObject.GetComponent<Image>();
        image.color = red;
        for (float i = .5f; i > .01f; i -= .01f) {
            yield return new WaitForSecondsRealtime(.01f);
            image.color = new Color(1, 0.24314f, 0.24314f, i);
        }
        gameObject.SetActive(false);
    }

    public IEnumerator Flash (Color color) {
        gameObject.SetActive(true);
        Image image = gameObject.GetComponent<Image>();
        image.color = color;
        for (float i = .5f; i > .01f; i -= .01f) {
            yield return new WaitForSecondsRealtime(.01f);
            color.a = i;
            image.color = color;
        }
        gameObject.SetActive(false);
    }

    //public IEnumerator IncreaseHealthFlash
}
