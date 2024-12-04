using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsDisplay : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    public static CoinsDisplay instance;
    public int coins;
    private TextMeshProUGUI coinsText;
    public event Action<int> coinUpdate;
    public void SaveData (GameData data) {
        data.coins = coins;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
    }

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start () {
        coinsText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("coins = "+coins);
        coinsText.text = "Coins: "+coins;
    }

    void Update () {
        coinsText.text = "Coins: "+coins; //jank and I know I could do a thing when I set coins but mehhhh
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            coins += 20; //TODO: REMOVE, replace with an audio output
            coinsText.text = "Coins: "+coins; //TODO: REMOVE
            //why would I remove that
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) { //TODO: REMOVE right button input completely
            coins = 0;
            coinsText.text = "Coins: "+coins;
        }
    }
}
