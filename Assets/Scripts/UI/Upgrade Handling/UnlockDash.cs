using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UnlockDash : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasDash;
    private TextMeshProUGUI hasDashText;
    public void SaveData (GameData data) {
        //Debug.Log("Saving... (UpgradeMapSize), hasDash = "+hasDash);
        data.hasDash = hasDash;
    }
    public void LoadData (GameData data) {
        hasDash = data.hasDash;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                hasDashText = textSet[i];
            }
        }
        Debug.Log("hasDash = "+hasDash);
        hasDashText.text = ""+hasDash;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (hasDash == false) {
                hasDash = true;
            } else {
                //play sound
            }
            hasDashText.text = ""+hasDash;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            hasDash = false;
            hasDashText.text = ""+hasDash;
        }
    }
}
