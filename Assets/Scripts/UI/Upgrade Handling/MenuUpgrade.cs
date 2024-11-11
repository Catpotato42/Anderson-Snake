using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUpgrade : MonoBehaviour, IPointerClickHandler
{
    void Start () {
        TextMeshProUGUI textSet = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        int current = PlayerPrefs.GetInt("mapSize");
        textSet.text = ""+(current);
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        TextMeshProUGUI textSet = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            int current = PlayerPrefs.GetInt("mapSize");
            PlayerPrefs.SetInt("mapSize", current + 1);
            textSet.text = ""+(current + 1);
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            PlayerPrefs.SetInt("mapSize", 0);
            textSet.text = ""+0;
        }
    }
}
