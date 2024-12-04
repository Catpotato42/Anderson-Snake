using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class UpgradeGrowAmount : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private int segmentsPerGrow;
    private TextMeshProUGUI segmentsPerGrowText;
    private int coins;
    public event Action<int> coinUpdate;
    private int[] costs = new int[]{
        100
    };
    public void SaveData (GameData data) {
        data.segmentsPerGrow = segmentsPerGrow;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
        segmentsPerGrow = data.segmentsPerGrow;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                segmentsPerGrowText = textSet[i];
            }
        }
        Debug.Log("segmentsPerGrow = "+segmentsPerGrow);
        segmentsPerGrowText.text = ""+segmentsPerGrow;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (segmentsPerGrow > 1) {
                segmentsPerGrow--;
            } else {
                //TODO: play sound
            }
            segmentsPerGrowText.text = ""+segmentsPerGrow;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            segmentsPerGrow = 3;
            segmentsPerGrowText.text = ""+segmentsPerGrow;
        }
    }
}
