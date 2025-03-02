using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradesScript : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    
    //The previous way I was doing this was possibly the worst way I did something in this project bar nothing. However, through this failure I practiced data structures so it's fine

    //Boy am I glad that this is basically the last thing in this project because THIS is spaghetti code. Unreadable, repetitive, messy... (◎﹏◎)

    //Alright I'm gonna delete all of these scripts and use this as a skeleton for the final upgrades script, but I'm gonna leave this record of
    //how horrible the old system was. (12/2/24)

    //bedtime but this needs to get done tmrw

    //Wow this is still not great

    //TODO BUGS: if the collection size is odd (maybe even?) for floats the max size gets activated one early

    //These are needed for all upgrades:
    private int coins;
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
    private TextMeshProUGUI costTracker;
    private int[] cost;
    int costSize;
    private TextMeshProUGUI amountText;
    Image buttonImage;
    [SerializeField] private string type;

    //For bools, default is always false.

    //bunch of variable definitions, they had to go somehwere.

    //Dash
    private bool hasDash;

    //Reverse
    private bool hasReverse;

    //Dash Invincibility
    //secondary unlock is in ScriptType as unlocked1, for example hasDash for Dash Invincibility
    private bool hasDashInvincibility;

    //Time Slow Unlock
    private bool hasTimeSlow;

    //Grow Amount
    private int segmentsPerGrow;
    private int defaultSegmentsPer = 4;
    private int minSegmentsPer = 1;

    //Extra Health Amount
    private float extraHealth;
    private float maxHealth = 1000f;
    private float defaultHealth = 0;
    private float healthIncrement = 50f;

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
    private float xpMultiMax = 2.5f;
    private float defaultXpMulti = 1f;
    private float xpIncrement = .1f;

    //Coin Multi
    private float coinMulti;
    private float maxCoinMulti = 4f;
    private float defaultCoinMulti = 1f;
    private float coinMultiIncrement = .1f;

    //Map Size
    private int mapSize;
    private int defaultMapSize = 0;
    private int maxMapSize = 15;

    //Extra Segments
    private int extraSegments;
    private int defaultExtraSegments = 10;
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

    //Time Slow Speed
    private float tsSpeed;
    private float minTSSpeed = .25f;
    private float defaultTSSpeed = .5f;
    private float tsSpeedIncrement = .05f;

    //Time Slow Cooldown
    private float tsCooldown; //inconsistent shortening of cooldown
    private float minTSCooldown = .5f;
    private float defaultTSCooldown = 2f;
    private float tsCDIncrement = .1f;

    //Dash Cooldown
    private float dashCD;
    private float minDashCD = .7f;
    private float defaultDashCD = 2f;
    private float dashCDIncrement = .1f;

    private AudioSource[] audioSources;
    private AudioSource change;
    private AudioSource locked;
    private AudioSource noMoney;

    public void SaveData (GameData data) {
        SaveType(ref data); //most stuff is auto passed as reference in C# but I don't think classes are - interesting
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
        coinMulti = data.coinMulti;
        mapSize = data.mapSize;
        extraSegments = data.extraSegments;
        extraChoices = data.extraChoices;
        tsLength = data.tsLength; //time
        tsSpeed = data.tsSpeed;
        tsCooldown = data.tsCooldown;
        dashCD = data.dashCD;
        ScriptType(); // ^_^
        OnStartAndReload();
    }

    private void SaveType (ref GameData data) {
        switch (type) {
            case "Dash":
                data.hasDash = unlocked0;
                break;
            case "DashInvincibility":
                data.hasDashInvincibility = unlocked0;
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
            case "CoinMulti":
                data.coinMulti = condFloat;
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
            case "DashCD":
                data.dashCD = condFloat;
                break;
            case "TimeSlowCooldown":
                data.tsCooldown = condFloat;
                break;
            case "TimeSlowSpeed":
                data.tsSpeed = condFloat;
                break; //USE COND___ NOT "variablename" FOR ALL SAVES
            default:
                Debug.Log("Error in SaveType: name = "+name);
                break;
        }
    }

    private void ScriptType () { //(ﾟДﾟ)
        switch (type) {
            case "Dash":
                unlocked0 = hasDash; //I would do this whole bit with pointers and return the actual variable in c++ for readability,
                // but c sharp doesn't like that.
                cost = new int[]{200}; //could use one of the unused int values but this is more consistent and readable.
                varType = 1;
                break;
            case "DashInvincibility":
                usingUnlock = true;
                unlocked1 = hasDash; //condition
                unlocked0 = hasDashInvincibility;
                cost = new int[]{120};
                varType = 1;
                break;
            case "Reverse":
                unlocked0 = hasReverse;
                cost = new int[]{200};
                varType = 1;
                break;
            case "TimeSlow":
                unlocked0 = hasTimeSlow;
                cost = new int[]{150};
                varType = 1;
                break;
            case "DecreaseGrowAmount":
                increment = false;
                condInt = segmentsPerGrow;
                boundaryInt = minSegmentsPer;
                defaultInt = defaultSegmentsPer;
                costSize = defaultInt - boundaryInt;
                cost = new int[costSize];
                cost[0] = 100; //I can forsee this being much easier if I assigned variables to costs with the others but honestly that might create even more clutter.
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 50;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 60;
                }
                varType = 2;
                break;
            case "ExtraHealth":
                increment = true;
                incrementAmount = healthIncrement;
                condFloat = extraHealth;
                boundaryFloat = maxHealth;
                defaultFloat = defaultHealth;
                costSize = (int)((boundaryFloat - defaultFloat) / incrementAmount);
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 50;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 10;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 20;
                }
                varType = 3;
                break;
            case "ExtraFood":
                increment = true;
                condInt = extraFood;
                defaultInt = defaultFood;
                boundaryInt = maxFood;
                costSize = boundaryInt - defaultInt;
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 40;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 5;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 7;
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
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 25;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 5;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 7;
                }
                varType = 3;
                break;
            case "XPMulti":
                increment = true;
                incrementAmount = xpIncrement;
                condFloat = xpMulti;
                boundaryFloat = xpMultiMax;
                defaultFloat = defaultXpMulti;
                costSize = (int)((boundaryFloat - defaultFloat) / incrementAmount);
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 15;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 3;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 5;
                }
                varType = 3;
                break;
            case "CoinMulti":
                increment = true;
                incrementAmount = coinMultiIncrement;
                condFloat = coinMulti;
                boundaryFloat = maxCoinMulti;
                defaultFloat = defaultCoinMulti;
                costSize = (int)((boundaryFloat - defaultFloat) / incrementAmount);
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 30;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 4;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 6;
                }
                varType = 3;
                break;
            case "MapSize":
                increment = true;
                condInt = mapSize;
                defaultInt = defaultMapSize;
                boundaryInt = maxMapSize;
                costSize = boundaryInt - defaultInt;
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 10;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 3;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 6;
                }
                varType = 2;
                break;
            case "RemoveSegments":
                increment = false;
                condInt = extraSegments;
                defaultInt = defaultExtraSegments;
                boundaryInt = minExtraSegments;
                costSize = defaultInt - boundaryInt;
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 25;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 10;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 15;
                }
                varType = 2;
                break;
            case "ExtraChoices":
                increment = true;
                condInt = extraChoices;
                defaultInt = defaultExtraChoices;
                boundaryInt = maxExtraChoices;
                costSize = boundaryInt - defaultInt;
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 100;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 100;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 200;
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
                costSize = (int)((boundaryFloat - defaultFloat) / incrementAmount); // 3 - 2 = 1 / .1 = 10
                cost = new int[costSize]; //make sure this can always convert to an int
                cost[0] = 15;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 3;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 5;
                }
                varType = 3;
                break;
            case "DashCD":
                increment = false;
                usingUnlock = true;
                unlocked1 = hasDash;
                incrementAmount = dashCDIncrement;
                condFloat = dashCD;
                boundaryFloat = minDashCD;
                defaultFloat = defaultDashCD;
                costSize = (int)((defaultFloat - boundaryFloat) / incrementAmount);
                cost = new int[costSize];
                cost[0] = 15;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 3;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 5;
                }
                varType = 3;
                break;
            case "TimeSlowCooldown":
                increment = false;
                usingUnlock = true;
                unlocked1 = hasTimeSlow;
                incrementAmount = tsCDIncrement;
                condFloat = tsCooldown;
                boundaryFloat = minTSCooldown;
                defaultFloat = defaultTSCooldown;
                costSize = (int)((defaultFloat - boundaryFloat) / incrementAmount);
                cost = new int[costSize];
                cost[0] = 50;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 10;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 15;
                }
                varType = 3;
                break;
            case "TimeSlowSpeed":
                increment = false;
                usingUnlock = true;
                unlocked1 = hasTimeSlow;
                incrementAmount = tsSpeedIncrement;
                condFloat = tsSpeed;
                boundaryFloat = minTSSpeed;
                defaultFloat = defaultTSSpeed;
                costSize = (int)((defaultFloat - boundaryFloat) / incrementAmount); //2-.7f = 1.3f / .1f = 13
                cost = new int[costSize]; //make sure this can always convert to an int, size 13
                cost[0] = 25;
                for (int i = 1; i < cost.Length/2; i++) {
                    cost[i] = cost[i-1] + 5;
                }
                for (int i = cost.Length / 2; i < cost.Length; i++) {
                    cost[i] = cost[i-1] + 10;
                }
                varType = 3;
                break;
            default:
                Debug.Log("Error in ScriptType: name = "+name);
                break;
        }
    }
    void Start () {
        //audio stuff
        audioSources = GetComponents<AudioSource>();
        foreach (AudioSource temp in audioSources) {
            if (temp.clip.name == "Locked") { //it is != Locked because then we can have buttons without the locked audio sound needed
                locked = temp;
            } else if (temp.clip.name == "noMoney") {
                noMoney = temp;
            } else if (temp.clip.name == "defaultButtonClick") {
                change = temp;
            } else {
                Debug.Log("Error in Start: temp.clip.name = "+temp.clip.name);
            }
        }
        OnStartAndReload();
        int onClickCostIndex = -1;
        switch (varType) {
            case 1:
                if (!unlocked0) {
                    costTracker.text = "Cost: " + cost[0];
                } else {
                    costTracker.text = "";
                }
                amountText.text = unlockString;
                break;
            case 2:
                DefineCostIndexInt(ref onClickCostIndex);
                if (onClickCostIndex > costSize - 1) {
                    costTracker.text = "MAX";
                } else {
                    costTracker.text = "Cost: " + cost[onClickCostIndex];
                }
                amountText.text = "" + condInt;
                break;
            case 3:
                DefineCostIndexFloat(ref onClickCostIndex);
                if (onClickCostIndex > costSize - 1) {
                    costTracker.text = "MAX";
                } else {
                    costTracker.text = "Cost: " + cost[onClickCostIndex];
                }
                amountText.text = "" + condFloat;
                break;
            default:
                Debug.Log("varType out of bounds. Start method UpgradesScript. varType: "+varType+". Type: "+type);
                break;
        }
    }

    private void OnStartAndReload () { //split off so that I can recheck for unlocks after a load, not everything needs to be in here but idc until I make the reload an event Action.
        TextMeshProUGUI[] textSet = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; i++) {
            if (textSet[i].text == "") {
                amountText = textSet[i];
            } else if (textSet[i].text == "cost") {
                costTracker = textSet[i];
            }
        }
        buttonImage = gameObject.GetComponent<Image>();
        buttonImage.color = Color.white; //so they are all a uniform white
        if (usingUnlock) {
            if (!unlocked1) {
                buttonImage.color = Color.gray;
            } else {
                buttonImage.color = Color.white;
            }
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
            Debug.Log("We don't actually support that button here.");
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

    private void OnClickBool () {
        if (coins >= cost[0]) {
            if (usingUnlock && !unlocked1) { //too nested ew, but if need unlock and unlock condition not met
                locked.Play();
                return;
            }
            if (unlocked0) {
                //nothing happens, already unlocked: TODO: either add a sound or make it revert when you click this
                return;
            }
            change.Play();
            CoinsDisplay.instance.coins -= cost[0];
            unlocked0 = true;
            amountText.text = unlockString;
            costTracker.text = "";
            if (type == "Dash" || type == "TimeSlow") { // ¯\(°_o)/¯ what else am I supposed to use like an event Action that all the usingUnlock scripts subscribe to? Ridiculous.
                SaveManager.instance.SaveGame();
                SaveManager.instance.LoadGame();
            }
        } else {
            noMoney.Play(); //BROKE (╯▽╰ )
            Debug.Log("Not enough money, cost = "+cost[0]);
        }
    }

    private void DefineCostIndexInt (ref int onClickCostIndex) {
         if (increment) {
            onClickCostIndex = condInt - defaultInt;
        } else {
            onClickCostIndex = defaultInt - condInt;
        }
    }

    private void OnClickInt () { //if increment, condInt - defaultInt
        int onClickCostIndex = -1; //why is this this way, TODO: fix whatever causes these floats and ints to go out of where they're supposed to be, ALSO it could be caused
        DefineCostIndexInt(ref onClickCostIndex); //by an interaction where you click it while it's locked and it still increments a variable.
        if (onClickCostIndex > costSize - 1) {
            return;
        }
        if (usingUnlock && !unlocked1) {
            locked.Play();
            return;
        }
        if (coins >= cost[onClickCostIndex]) {
            if (increment && condInt < boundaryInt) { //if incrementing and below the boundary, these two seem redundant due to the cost index
                condInt++;
            } else if (!increment && condInt > boundaryInt) { //if decrementing, above boundary
                condInt--;
            } else { //going above or below would be out of bounds
                return;
            }
            change.Play();
            CoinsDisplay.instance.coins -= cost[onClickCostIndex];
            amountText.text = ""+condInt;
            onClickCostIndex++;
            if (onClickCostIndex > costSize - 1) {
                costTracker.text = "MAX";
            } else {
                costTracker.text = "Cost: "+cost[onClickCostIndex];
            }
        } else {
            noMoney.Play();
            Debug.Log("Not enough money, cost = "+cost[onClickCostIndex]);
        }
    }

    private void DefineCostIndexFloat (ref int onClickCostIndex) {
        if (increment) {
            onClickCostIndex = (int)((condFloat - defaultFloat) / incrementAmount);
        } else {
            onClickCostIndex = (int)((defaultFloat - condFloat) / incrementAmount);
        }
    }

    private void OnClickFloat () {
        int onClickCostIndex = -1;
        DefineCostIndexFloat(ref onClickCostIndex);
        if (onClickCostIndex > costSize - 1) {
            return;
        }
        if (usingUnlock && !unlocked1) {
            locked.Play();
            return;
        }
        if (coins >= cost[onClickCostIndex]) {
            if (increment && condFloat < boundaryFloat) { //if incrementing and below the boundary
                condFloat += incrementAmount;
            } else if (!increment && condFloat > boundaryFloat) { //if decrementing, above boundary
                condFloat -= incrementAmount;
            } else { //going above or below would be out of bounds
                return;
            }
            change.Play();
            CoinsDisplay.instance.coins -= cost[onClickCostIndex];
            condFloat = (float)Math.Round(condFloat, 3);
            amountText.text = ""+condFloat;
            onClickCostIndex++;
            if (onClickCostIndex >= costSize) { //maybe root of the problem?
                costTracker.text = "MAX";
            } else {
                costTracker.text = "Cost: "+cost[onClickCostIndex];
            }
        } else {
            noMoney.Play();
            Debug.Log("Not enough money, cost = "+cost[onClickCostIndex]);
        }
    }

    private void RightClickLogic () { //TODO: comment out on release version
        int onClickCostIndex = -1;
        switch(varType) {
            case 1:
                unlocked0 = false;
                amountText.text = unlockString;
                costTracker.text = "Cost: "+cost[0];
                if (type == "Dash" || type == "TimeSlow") {
                    SaveManager.instance.SaveGame();
                    SaveManager.instance.LoadGame();
                }
                break;
            case 2:
                condInt = defaultInt;
                amountText.text = condInt + "";
                DefineCostIndexInt(ref onClickCostIndex); //should always be 0, just an extra fail point if things are wonky
                costTracker.text = "Cost: " + cost[onClickCostIndex];
                break;
            case 3:
                condFloat = defaultFloat;
                amountText.text = ""+condFloat;
                DefineCostIndexFloat(ref onClickCostIndex); //should also always be 0
                costTracker.text = "Cost: " + cost[onClickCostIndex];
                break;
            default:
                Debug.Log("error: varType, UpgradesScript RightClickLogic. varType: "+varType);
                break;
        }
    }
}
