using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{

    //currently useless until I find errors I want to display, use this syntax to display:
    //in variable declaration:
    //[SerializeField] private GameObject errorPanel;
    //private ErrorHandler errorHandler;
    //in start or awake:
    //if (errorPanel != null) { 
    //  errorHandler = errorPanel.GetComponent<ErrorHandler>();
    //}
    //where you want the error to display:
    //StartCoroutine(errorHandler.ShowError(string I want to display));
    void Awake()
    {
        SetVisible(false);
    }

    public IEnumerator ShowError(string message) {
        TextMeshProUGUI textElement = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textElement.text = message;
        for (int i = 0; i < 4; i++) {
            SetVisible(true);
            yield return new WaitForSecondsRealtime(.7f);
            SetVisible(false);
            yield return new WaitForSecondsRealtime(.4f);
        }
    }

    void SetVisible(bool active) {
        TextMeshProUGUI textElement = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        Image image = gameObject.GetComponent<Image>();
        textElement.enabled = active;
        image.enabled = active;
    }
}
