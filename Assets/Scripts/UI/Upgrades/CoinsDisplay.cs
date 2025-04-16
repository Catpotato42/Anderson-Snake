using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CoinsDisplay : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    public static CoinsDisplay instance;
    public int coins;
    private TextMeshProUGUI coinsText;
    [SerializeField] private GameObject goldAcorn; 
    private bool acornFound;
    private bool canSpawnAcorn = true;
    public void SaveData (GameData data) {
        data.coins = coins;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
        acornFound = data.acorn3;
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
            coins += 1000; //TODO: REMOVE, replace with an audio output
            coinsText.text = "Coins: "+coins; //TODO: REMOVE
            AudioManager.instance.PlayAudio("defaultButtonClick");
            if (SceneManager.GetActiveScene().buildIndex == 4 && !acornFound && canSpawnAcorn) {
                canSpawnAcorn = false;
                Debug.Log("acornFound = "+acornFound+", spawning acorn");
                GameObject acorn = Instantiate(goldAcorn, new Vector3(7f, 1.5f, 0f), Quaternion.identity);
                acorn.transform.localScale = new Vector3 (2,2,0);
                acorn.name = "Acorn3";
                SaveManager.instance.UpdateSaveManagerObjects();
                SaveManager.instance.LoadGame();
            }
        } /*else if (pointerEventData.button == PointerEventData.InputButton.Right) { //TODO: REMOVE right button input completely
            coins = 0;
            coinsText.text = "Coins: "+coins;
        }*/
    }
}
