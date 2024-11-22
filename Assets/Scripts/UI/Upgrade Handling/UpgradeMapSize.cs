using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UpgradeMapSize : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    int mapSize;
    public void SaveData (GameData data) {
        Debug.Log("Saving... (UpgradeMapSize), mapSize = "+mapSize);
        data.mapSize = this.mapSize;
    }
    public void LoadData (GameData data) {
        this.mapSize = data.mapSize;
    }
    void Start () {
        TextMeshProUGUI textSet = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(LateStart(textSet));
        Debug.Log("mapSize = "+mapSize);
    }

    IEnumerator LateStart(TextMeshProUGUI textSet)
    {
        yield return new WaitForSeconds(.03f);
        textSet.text = ""+mapSize;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        TextMeshProUGUI textSet = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            mapSize++;
            textSet.text = ""+mapSize;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            mapSize = 0;
            textSet.text = ""+mapSize;
        }
    }
}
