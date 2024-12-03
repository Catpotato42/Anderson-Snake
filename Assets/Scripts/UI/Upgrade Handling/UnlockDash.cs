using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UnlockDash : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    //holy SHIT i regret not doing these as a single script just when I thought about the switch case I would do I suddenly had flashbacks
    //to a video essay about yanderedev's code and I was like nah fuck that but it LITERALLY would have been two switch cases at the start of the code
    //and the onpointerclick plus a little extra checking stuff so much regret is within me (★‿★)

    //Ok the honestly scuffed but pretty cool solution I have found is to make UnlockDash's Start method run before the other Upgrade things and then the other scripts
    //have a reference to this one and get the list of Actions from this script so we don't have to run this in every script
    
    //This was possibly the worst way I did something in this project bar nothing. However, through this I practiced data structures so it's fine

    //Boy am I glad that this is basically the last thing in this project because THIS is spaghetti code. Unreadable, repetitive, messy... (◎﹏◎)
    private bool hasDash;
    private TextMeshProUGUI hasDashText;
    private int coins;
    public event Action<int> coinUpdate;
    public List<Action<int>> coinUpdateList = new List<Action<int>>();
    private int[] costs = new int[]{
        100
    };
    private string hasDashString {
        get {
            if (hasDash) {
                return "Unlocked";
            } else {
                return "Locked";
            }
        }
    }
    public void SaveData (GameData data) {
        data.hasDash = hasDash;
    }
    public void LoadData (GameData data) {
        coins = data.coins;
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
        hasDashText.text = hasDashString;
        //Getting list of event Actions, which is defined up top.

        //all objects
        UnityEngine.Object[] objectsInScene = FindObjectsOfType(typeof(MonoBehaviour));
        //the scripts
        List<MonoBehaviour> scripts = new List<MonoBehaviour>();
        //go through objects and add their scripts if they have any to my scripts list, which must be expandable. 
        foreach (var objectInScene in objectsInScene) {
            if (objectInScene.GetComponent<MonoBehaviour>() != null) {
                scripts.Add(objectInScene.GetComponent<MonoBehaviour>());
            }
        }
        //now getting the actions from the scripts
        foreach (var script in scripts)
        {
            if (true)
            {
                coinUpdateList.Add((Action<int>)Delegate.CreateDelegate(typeof(Action<int>), script, "coinUpdate"));
                Action<int> coinUpdateAction = (Action<int>)Delegate.CreateDelegate(typeof(Action<int>), script, "coinUpdate");
                //in ALL upgrade scripts and CoinsDisplay
                coinUpdateAction += UpdateCoins;
                //
            }
        }
    }

    private void UpdateCoins (int newCoinAmount) {
        coins = newCoinAmount;
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (!hasDash && coins > costs[0]) {
                hasDash = true;
                coins -= costs[0];
                coinUpdate.Invoke(coins);
            } else {
                //play sound
            }
            hasDashText.text = hasDashString;
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            hasDash = false;
            hasDashText.text = hasDashString;
        }
    }
}
