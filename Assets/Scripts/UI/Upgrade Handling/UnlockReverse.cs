using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UnlockReverse : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasReverse;
    private TextMeshProUGUI hasReverseText;
    public void SaveData (GameData data) {
        //Debug.Log("Saving... (UpgradeMapSize), hasReverse = "+hasReverse);
        data.hasReverse = hasReverse;
    }
    public void LoadData (GameData data) {
        hasReverse = data.hasReverse;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                hasReverseText = textSet[i];
            }
        }
        Debug.Log("hasReverse = "+hasReverse);
        hasReverseText.text = ""+hasReverse;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (hasReverse == false) { 
                hasReverse = true;
            } else {
                //TODO: play sound
            }
            hasReverseText.text = ""+hasReverse;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            hasReverse = false;
            hasReverseText.text = ""+hasReverse;
        }
    }
}
