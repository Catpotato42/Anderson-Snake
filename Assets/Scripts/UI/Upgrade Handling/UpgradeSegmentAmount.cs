using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UpgradeSegmentAmount : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private int extraSegments;
    private TextMeshProUGUI extraSegmentsText;
    public void SaveData (GameData data) {
        //Debug.Log("Saving... (UpgradeMapSize), extraSegments = "+extraSegments);
        data.extraSegments = extraSegments;
    }
    public void LoadData (GameData data) {
        extraSegments = data.extraSegments;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                extraSegmentsText = textSet[i];
            }
        }
        Debug.Log("extraSegments = "+extraSegments);
        extraSegmentsText.text = ""+extraSegments;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (extraSegments > 0) {
                extraSegments--;
            } else {
                //TODO: play sound
                Debug.Log("Can't go below "+extraSegments+" extra segments.");
            }
            extraSegmentsText.text = ""+extraSegments;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            extraSegments = 10;
            extraSegmentsText.text = ""+extraSegments;
        }
    }
}
