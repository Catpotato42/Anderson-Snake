using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class UpgradeTimeAmount : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private float runTime;
    private TextMeshProUGUI timeAmountText;
    private int coins;
    public event Action<int> coinUpdate;
    private int[] costs = new int[]{
        100
    };
    public void SaveData (GameData data) {
        data.runTime = runTime;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
        runTime = data.runTime;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                timeAmountText = textSet[i];
            }
        }
        Debug.Log("runTime = "+runTime);
        timeAmountText.text = ""+runTime;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            runTime++;
            timeAmountText.text = ""+runTime;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            runTime = 20;
            timeAmountText.text = ""+runTime;
        }
    }
}
