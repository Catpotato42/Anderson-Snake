using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UpgradesScript : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    //holy SHIT i regret not doing these as a single script just when I thought about the switch case I would do I suddenly had flashbacks
    //to a video essay about yanderedev's code and I was like nah fuck that but it LITERALLY would have been two switch cases at the start of the code
    //and the onpointerclick plus a little extra checking stuff so much regret is within me (★‿★)

    //Ok the honestly scuffed but pretty cool solution I have found is to make UnlockDash's Start method run before the other Upgrade things and then the other scripts
    //have a reference to this one and get the list of Actions from this script so we don't have to run this in every script
    
    //This was possibly the worst way I did something in this project bar nothing. However, through this I practiced data structures so it's fine

    //Boy am I glad that this is basically the last thing in this project because THIS is spaghetti code. Unreadable, repetitive, messy... (◎﹏◎)

    //Alright I'm gonna delete all of these scripts and use this as a skeleton for the final upgrades script, but I'm gonna leave this record of
    //how horrible the old system was. (12/2/24)

    //bedtime but this needs to get done tmrw

    //Needs to get coin amount from CoinDisplay every frame

    //These are needed for all upgrades:
    private int coins;
    public event Action<int> coinUpdate;
    private int varType;
    private string unlockString {
        get {
            if (unlocked0) {
                return "Unlocked";
            } else {
                return "Locked";
            }
        }
    } //for the bools, types unlocked/locked instead of true/false
    private bool usingUnlock = false; //type 1, could've used increment if I wanted but for clarity we'll make this usingUnlock bool.
    private bool unlocked0 = false; //condBool
    private bool unlocked1 = false; //condition for being able to unlock upgrade if usingUnlock is true
    private int condInt; //type 2
    private int defaultInt;
    private int boundaryInt;
    private float condFloat; //type 3
    private float defaultFloat;
    private float boundaryFloat;
    private bool increment; //if we increment or decrement, relevant for ints and floats
    private float incrementAmount; //basically for ints this is always 1 so we don't need to check
    private int costTracker;
    private List<int> cost;
    int costSize;
    private TextMeshProUGUI amountText;
    [SerializeField] private string type;

    //For bools, default is always false.

    //Dash
    private bool hasDash;

    //Reverse
    private bool hasReverse;

    //Dash Invincibility
    //private bool hasDash
    private bool hasDashInvincibility;

    //Time Slow Unlock
    private bool hasTimeSlow;

    //Grow Amount
    private int segmentsPerGrow;
    private int defaultSegmentsPer = 3;
    private int minSegmentsPer = 1;

    //Extra Health Amount
    private float extraHealth;
    private float maxHealth = 2000f;
    private float defaultHealth = 0;
    private float healthIncrement = 25f;

    //Extra Food Amount
    private int extraFood;
    private int defaultFood = 0;
    private int maxFood = 10;

    //Time Amount
    private float runTime;
    private float defaultTime = 20;
    private float maxTime = 300;
    private float timeIncrement = 5f;

    //XP Multi
    private float xpMulti;
    private float xpMultiMax = 6f;
    private float defaultXpMulti = 1f;
    private float xpIncrement = .1f;

    //Map Size
    private int mapSize;
    private int defaultMapSize = 0;
    private int maxMapSize = 300;

    //Extra Segments
    private int extraSegments;
    private int defaultExtraSegments = 5;
    private int minExtraSegments = 0;

    //Extra Choices
    private int extraChoices;
    private int defaultExtraChoices = 0;
    private int maxExtraChoices = 2;

    //Time Slow Length
    private float tsLength;
    private float maxTSLength = 3f;
    private float defaultTSLength = 2f;
    private float tsIncrement = .1f;



    public void SaveData (GameData data) {
        SaveType(data); //this should be a reference, I think cause most stuff is auto passed as reference in C# it's fine
    }
    public void LoadData (GameData data) {
        hasDash = data.hasDash;
        hasDashInvincibility = data.hasDashInvincibility;
        hasReverse = data.hasReverse;
        hasTimeSlow = data.hasTimeSlow;
        segmentsPerGrow = data.segmentsPerGrow;
        extraHealth = data.extraHealth;
        extraFood = data.extraFood;
        runTime = data.runTime;
        xpMulti = data.xpMulti;
        mapSize = data.mapSize;
        extraSegments = data.extraSegments;
        extraChoices = data.extraChoices;
        tsLength = data.tsLength; //time slow
    }

    private void SaveType (GameData data) {
        switch (type) {
            case "Dash":
                data.hasDash = unlocked0;
                break;
            case "DashInvincibility":
                data.hasDashInvincibility = unlocked1;
                break;
            case "TimeSlow":
                data.hasTimeSlow = unlocked0;
                break;
            case "Reverse":
                data.hasReverse = unlocked0;
                break;
            case "DecreaseGrowAmount":
                data.segmentsPerGrow = condInt;
                break;
            case "ExtraHealth":
                data.extraHealth = condFloat;
                break;
            case "ExtraFood":
                data.extraFood = condInt;
                break;
            case "RunTime":
                data.runTime = condFloat;
                break;
            case "XPMulti":
                data.xpMulti = condFloat;
                break;
            case "MapSize":
                data.mapSize = condInt;
                break;
            case "RemoveSegments":
                data.extraSegments = condInt;
                break;
            case "ExtraChoices":
                data.extraChoices = condInt;
                break;
            case "TimeSlowLength":
                data.tsLength = condFloat;
                break;
            default:
                Debug.Log("Error in SaveType: name = "+name);
                break;
        }
    }

    private void ScriptType () { //could do this on load for efficiency but this makes it maybe a little more readable is what I'm telling myself (ﾟДﾟ)
        switch (type) {
            case "Dash":
                unlocked0 = hasDash; //I would do this whole bit with pointers and return the actual variable in c++ for readability,
                // but c sharp doesn't like that.
                cost = new List<int>{200}; //could use one of the unused int values but this is more consistent and readable.
                varType = 1;
                break;
            case "DashInvincibility":
                usingUnlock = true;
                unlocked1 = hasDash; //condition
                unlocked0 = hasDashInvincibility;
                cost = new List<int>{120};
                varType = 1;
                break;
            case "Reverse":
                unlocked0 = hasReverse;
                cost = new List<int>{200};
                varType = 1;
                break;
            case "TimeSlow":
                unlocked0 = hasTimeSlow;
                cost = new List<int>{150};
                varType = 1;
                break;
            case "DecreaseGrowAmount":
                increment = false;
                condInt = segmentsPerGrow;
                defaultInt = defaultSegmentsPer;
                boundaryInt = minSegmentsPer;
                costSize = 2;
                cost = new List<int>{50, 60}; //+3->+2, +2->+1, can't go past that
                varType = 2;
                break;
            case "ExtraHealth":
                increment = true;
                incrementAmount = healthIncrement;
                condFloat = extraHealth;
                boundaryFloat = maxHealth;
                defaultFloat = defaultHealth;
                costSize = (int)((boundaryFloat - defaultFloat) / incrementAmount);
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(50);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 10);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 20);
                }
                varType = 3;
                break;
            case "ExtraFood":
                increment = true;
                condInt = extraFood;
                defaultInt = defaultFood;
                boundaryInt = maxFood;
                costSize = boundaryInt - defaultInt;
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(40);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 5);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 7);
                }
                varType = 2;
                break;
            case "RunTime":
                increment = true;
                incrementAmount = timeIncrement;
                condFloat = runTime;
                boundaryFloat = maxTime;
                defaultFloat = defaultTime;
                costSize = (int)((boundaryFloat - defaultFloat) / incrementAmount);
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(25);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 5);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 7);
                }
                varType = 3;
                break;
            case "XPMulti":
                increment = true;
                incrementAmount = xpIncrement;
                condFloat = xpMulti;
                boundaryFloat = xpMultiMax;
                defaultFloat = defaultXpMulti;
                costSize = (int)(((boundaryFloat - defaultFloat) / incrementAmount) - defaultFloat);
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(15);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 3);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 5);
                }
                varType = 3;
                break;
            case "MapSize":
                increment = true;
                condInt = mapSize;
                defaultInt = defaultMapSize;
                boundaryInt = maxMapSize;
                costSize = boundaryInt - defaultInt;
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(10);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 3);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 6);
                }
                varType = 2;
                break;
            case "RemoveSegments":
                increment = false;
                condInt = extraSegments;
                defaultInt = defaultExtraSegments;
                boundaryInt = minExtraSegments;
                costSize = boundaryInt - defaultInt;
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(25);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 10);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 15);
                }
                varType = 2;
                break;
            case "ExtraChoices":
                increment = true;
                condInt = extraChoices;
                defaultInt = defaultExtraChoices;
                boundaryInt = maxExtraChoices;
                costSize = boundaryInt - defaultInt;
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(100);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 100);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 200);
                }
                varType = 2;
                break;
            case "TimeSlowLength":
                increment = true;
                usingUnlock = true;
                unlocked1 = hasTimeSlow;
                incrementAmount = tsIncrement;
                condFloat = tsLength;
                boundaryFloat = maxTSLength;
                defaultFloat = defaultTSLength;
                costSize = (int)(((boundaryFloat - defaultFloat) / incrementAmount) - defaultFloat);
                cost = new List<int>(costSize); //make sure this can always convert to an int
                cost.Add(15);
                for (int i = 1; i < cost.Count/2; i++) {
                    cost.Add(cost[i-1] + 3);
                }
                for (int i = cost.Count / 2; i < cost.Count; i++) {
                    cost.Add(cost[i-1] + 5);
                }
                varType = 3;
                break;
            default:
                Debug.Log("Error in ScriptType: name = "+name);
                break;
        }
    }

    void Start () {
        ScriptType();
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                amountText = textSet[i];
            }
        }
        switch (varType) {
            case 1:
                amountText.text = unlockString;
                break;
            case 2:
                amountText.text = "" + condInt;
                break;
            case 3:
                amountText.text = "" + condFloat;
                break;
            default:
                Debug.Log("varType out of bounds. Start method UpgradesScript. varType: "+varType);
                break;
        }
    }

    void Update () {
        //get coin amount from CoinsDisplay
        coins = CoinsDisplay.instance.coins;
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            LeftClickLogic();
        } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            RightClickLogic();
        } else {
            Debug.Log("Sir, we don't actually support that button here. You'll have to go to a different scr- no I can't do that either, we have- xbox controller? Get out. Get the fuck out.");
        }
    }

    private void LeftClickLogic () {
        switch (varType) {
            case 1:
                OnClickBool();
                break;
            case 2:
                OnClickInt();
                break;
            case 3:
                OnClickFloat();
                break;
        }
    }

    private void OnClickBool () { //working for 1 cond, need to test 2
        if (coins >= cost[0]) {
            if (usingUnlock && !unlocked1) { //too nested ew, but if need cond and cond not met
                //TODO: play sound for locked, should be different than not enough money
                return;
            }
            if (unlocked0) {
                //nothing happens, already unlocked
                return;
            }
            CoinsDisplay.instance.coins -= cost[0];
            unlocked0 = true;
            amountText.text = unlockString;
        } else {
            Debug.Log("Not enough money, cost = "+cost[0]);
        }
    }

    private void UsingUnlockOnClick () {
        
    }

    private void OnClickInt () {
        
    }

    private void OnClickFloat () {
        
    }

    private void RightClickLogic () {
        switch(varType) {
            case 1:
                unlocked0 = false;
                amountText.text = unlockString;
                break;
            case 2:
                condInt = defaultInt;
                amountText.text = condInt + "";
                break;
            case 3:
                condFloat = defaultFloat;
                amountText.text = ""+condFloat;
                break;
            default:
                Debug.Log("error: varType, UpgradesScript RightClickLogic. varType: "+varType);
                break;
        }
    }
}
