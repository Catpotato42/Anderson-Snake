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
        this.extraHealth = 0; //is "this." needed?
        this.extraChoices = 0;
        this.extraSegments = 0;
        segmentsPerGrow = 3; //higher is worse
        mapSize = 0;
        runTime = 70;
        hasDash = false; //
        hasReverse = false;
        hasTimeSlow = false;
        hasDashInvincibility = false; //
        hasMedium = false;
        hasHard = false;
        hasEverett = false;
        highScoreB = 0;
        highScoreM = 0;
        highScoreH = 0;
        highScoreEv = 0;
        permanentDisallowedUpgrades = new SerializableHashSet<int, int>
        {
            new Tuple<int, int>(5, 0),
            new Tuple<int, int>(4, 1),
            new Tuple<int, int>(3, 1),
        };
    }
}
