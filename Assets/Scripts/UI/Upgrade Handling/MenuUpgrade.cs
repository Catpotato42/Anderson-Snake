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
        int current = PlayerPrefs.GetInt("mapSize");
        PlayerPrefs.SetInt("mapSize", current + 1);
        textSet.text = ""+(current + 1);
    }
}
