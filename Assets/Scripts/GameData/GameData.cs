using System;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int extraHealth;
    public int mapSize;
    public bool hasMedium;
    public bool hasHard;
    public bool hasEverett;
    public int extraSegments;
    public int extraFood;
    public int extraChoices;
    public int highScoreB;
    public int highScoreM;
    public int highScoreH;
    public int highScoreEv;
    public SerializableHashSet<int, int> permanentDisallowedUpgrades;
    //add: segment amount
    //add: skin preference
    //add: meta currency amount, make sure it is equal to currency + current score and if no current score is found just currency
    public GameData () {
        this.extraHealth = 0;
        extraChoices = 0;
        extraSegments = 0;
        mapSize = 0;
        hasMedium = true;
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
