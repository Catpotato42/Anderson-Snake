using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System;

public class UpgradeExtraHealthAmount : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private float extraHealth;
    private TextMeshProUGUI extraHealthText;
    private int coins;
    public event Action<int> coinUpdate;
    private int[] costs = new int[]{
        100
    };
    public void SaveData (GameData data) {
        data.extraHealth = extraHealth;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
        extraHealth = data.extraHealth;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                extraHealthText = textSet[i];
            }
        }
        Debug.Log("extraHealth = "+extraHealth);
        extraHealthText.text = ""+extraHealth;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (extraHealth < 1000) {
                extraHealth += 10;
            } else {
                //TODO: play sound
            }
            extraHealthText.text = ""+extraHealth;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            extraHealth = 0;
            extraHealthText.text = ""+extraHealth;
        }
    }
}
