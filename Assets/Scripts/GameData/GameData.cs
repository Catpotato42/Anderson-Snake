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
    //Trackers
    public bool hasMedium;
    public bool hasHard;
    public bool hasEverett;
    public int highScoreB;
    public int highScoreM;
    public int highScoreH;
    public int highScoreEv;
    public SerializableHashSet<int, int> permanentDisallowedUpgrades;
    //add: segment amount
    //add: skin preference
    //add: meta currency amount, make sure it is equal to currency + current score and if no current score is found just currency
    public GameData () {
        this.extraHealth = 250; //is "this." needed?
        this.extraChoices = 0;
        this.extraSegments = 0;
        segmentsPerGrow = 2; //higher is worse
        xpMulti = 1;
        mapSize = 0;
        runTime = 30;
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
        permanentDisallowedUpgrades = new SerializableHashSet<int, int>
        {

        };
    }
}
