using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsDisplay : MonoBehaviour, IPointerClickHandler, ISaveManager
{

    private int coins;
    UnlockDash unlockDash;
    private TextMeshProUGUI coinsText;
    public event Action<int> coinUpdate;
    public void SaveData (GameData data) {
        data.coins = coins;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
    }

    void Start () {
        coinsText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("coins = "+coins);
        coinsText.text = "Coins: "+coins;
        unlockDash = GameObject.FindGameObjectWithTag("CoinLeader").GetComponent<UnlockDash>();
        for (int i = 0; i < unlockDash.coinUpdateList.Count; i++) {
            unlockDash.coinUpdateList[i] += UpdateCoins;
        }
    }

    private void UpdateCoins (int newCoinAmount) {
        coins = newCoinAmount;
        coinsText.text = "Coins: "+coins;
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            coins += 20; //TODO: REMOVE, replace with an audio output
            coinsText.text = "Coins: "+coins; //TODO: REMOVE
            coinUpdate.Invoke(coins);
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) { //TODO: REMOVE right button input completely
            coins = 0;
            coinsText.text = "Coins: "+coins;
            coinUpdate.Invoke(coins);
        }
    }
}
