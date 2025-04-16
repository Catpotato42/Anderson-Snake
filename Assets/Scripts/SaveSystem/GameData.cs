using System;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    //TODO: Add some disallowed upgrades!
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
    public float coinMulti;
    public float tsLength; //time slow length
    public float tsSpeed;
    public float tsCooldown;
    public float dashCD;
    public float reverseCD;
    //Trackers
    public float speedrunTimer;
    public bool timerDone;
    public bool hasMedium;
    public bool hasHard;
    public bool hasEverett;
    public bool hasEverettSkin;
    public bool challengeUpgrades; //tracked with everett skin
    public bool hasChallengeRun;
    public bool allAcornsCollected;
    public bool acorn1;
    public bool acorn2;
    public bool acorn3;
    public bool acorn4;
    public bool acorn5;
    public int acornsCollected;
    public int highScoreB;
    public int highScoreM;
    public int highScoreH;
    public int highScoreEv;
    public int coins;
    public SerializableHashSet<int, int> permanentDisallowedUpgrades;
    //Options
    public string skinPref;
    public float volume;
    public GameData () { //constructor, flaw being that all changes to the values in here need to be echoed in UpgradesScript defaults.
        this.extraHealth = 0; //is "this." needed?
        this.extraChoices = 0;
        this.extraSegments = 10;
        extraFood = 0;
        segmentsPerGrow = 4; //higher is worse
        xpMulti = 1;
        coinMulti = 1;
        mapSize = 0;
        runTime = 10;
        tsLength = 2f;
        tsSpeed = 0.5f; //
        tsCooldown = 2f; //
        dashCD = 2f;
        reverseCD = 0.8f;
        hasDash = false; //
        hasReverse = false;
        hasTimeSlow = false;
        hasDashInvincibility = false; //
        speedrunTimer = 0f;
        timerDone = true;
        hasMedium = false;
        hasHard = false;
        hasEverett = false;
        hasEverettSkin = false;
        hasChallengeRun = false;
        challengeUpgrades = false;
        allAcornsCollected = false;
        acorn1 = acorn2 = acorn3 = acorn4 = acorn5 = false;
        acornsCollected = 0;
        highScoreB = 0;
        highScoreM = 0;
        highScoreH = 0;
        highScoreEv = 0;
        coins = 0;
        skinPref = "normal";
        volume = 0.5f; 
        permanentDisallowedUpgrades = new SerializableHashSet<int, int>
        {

        };
    }
}
