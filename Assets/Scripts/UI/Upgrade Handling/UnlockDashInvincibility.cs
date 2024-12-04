using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnlockDashInvincibility : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasDash;
    private bool hasDashInvincibility;
    private TextMeshProUGUI hasDashInvincibilityText;
    private int coins;
    public event Action<int> coinUpdate;
    private int[] costs = new int[]{
        100
    };
    private string hasDashInvincibilityString {
        get {
            if (hasDashInvincibility) {
                return "Unlocked";
            } else {
                return "Locked";
            }
        }
    }
    public void SaveData (GameData data) {
        data.hasDashInvincibility = hasDashInvincibility;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
        hasDash = data.hasDash;
        hasDashInvincibility = data.hasDashInvincibility;
    }
    void Start () {
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                hasDashInvincibilityText = textSet[i];
            }
        }
        Debug.Log("hasDashInvincibility = "+hasDashInvincibility);
        hasDashInvincibilityText.text = hasDashInvincibilityString;
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            SaveManager.instance.SaveGame(); //TODO: make this more efficient
            SaveManager.instance.LoadGame();
            if (hasDashInvincibility == false && hasDash) {
                hasDashInvincibility = true;
            } else {
                //play sound
            }
            hasDashInvincibilityText.text = hasDashInvincibilityString;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            hasDashInvincibility = false;
            hasDashInvincibilityText.text = hasDashInvincibilityString;
        }
    }
}
