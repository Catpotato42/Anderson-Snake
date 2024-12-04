using System;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    //Upgradables
    public float extraHealth;
    public int mapSize;
    public float runTime;
    public int extraSegments;
    public int extraFood;
    public int segmentsPerGrow;
    public int extraChoices;
    public bool hasDash;
    public bool hasReverse; //separate value for if it is bought
    public bool hasTimeSlow;
    public bool hasDashInvincibility;
    public float xpMulti;
    public float tsLength; //time slow length
    //Trackers
    public bool hasMedium;
    public bool hasHard;
    public bool hasEverett;
    public int highScoreB;
    public int highScoreM;
    public int highScoreH;
    public int highScoreEv;
    public int coins;
    public SerializableHashSet<int, int> permanentDisallowedUpgrades;
    //Options
    public string skinPref;
    //add: segment amount
    //add: skin preference
    //add: meta currency amount, make sure it is equal to currency + current score and if no current score is found just currency
    public GameData () {
        this.extraHealth = 0; //is "this." needed?
        this.extraChoices = 0;
        this.extraSegments = 5;
        segmentsPerGrow = 3; //higher is worse
        xpMulti = 1;
        mapSize = 0;
        runTime = 30;
        tsLength = 2f;
        hasDash = true; //
        hasReverse = true;
        hasTimeSlow = false;
        hasDashInvincibility = true; //
        hasMedium = false;
        hasHard = false;
        hasEverett = false;
        highScoreB = 0;
        highScoreM = 0;
        highScoreH = 0;
        highScoreEv = 0;
        coins = 0;
        skinPref = "normal";
        permanentDisallowedUpgrades = new SerializableHashSet<int, int>
        {

        };
    }
}
