using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnlockReverse : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasReverse;
    private string hasReverseString {
        get {
            if (hasReverse) {
                return "Unlocked";
            } else {
                return "Locked";
            }
        }
    }
    private TextMeshProUGUI hasReverseText;
    private int coins;
    UnlockDash unlockDash;
    public event Action<int> coinUpdate;
    private int[] costs = new int[]{
        120
    };
    public void SaveData (GameData data) {
        data.hasReverse = hasReverse;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
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
        hasReverseText.text = hasReverseString;
        unlockDash = GameObject.FindGameObjectWithTag("CoinLeader").GetComponent<UnlockDash>();
        for (int i = 0; i < unlockDash.coinUpdateList.Count; i++) {
            unlockDash.coinUpdateList[i] += UpdateCoins;
        }
    }

    private void UpdateCoins (int newCoinAmount) {
        coins = newCoinAmount;
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (!hasReverse && coins > costs[0]) { 
                hasReverse = true;
                coins -= costs[0];
                coinUpdate.Invoke(coins);
            } else {
                //TODO: play sound
            }
            hasReverseText.text = hasReverseString;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            hasReverse = false;
            hasReverseText.text = hasReverseString;
        }
    }
}
