using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using System.Collections;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    public class UpgradeInfo {
        private string name;
        private int level;
        private int rarity;

        public string Name {
            get => name;
            set {name = value;}
        }

        public int Level {
            get => level;
            set {level = value;}
        }

        public int Rarity {
            get => rarity;
            set {rarity = value;}
        }

        public UpgradeInfo(string name, int level, int rarity) {
            this.name = name;
            this.level = level;
            this.rarity = rarity;
        }
    }
    

    //upgrades stuff
    public Dictionary<int, UpgradeInfo> upgrades0 = new Dictionary<int, UpgradeInfo>();
    public Dictionary<int, UpgradeInfo> upgrades1 = new Dictionary<int, UpgradeInfo>();
    public Dictionary<int, UpgradeInfo> upgrades2 = new Dictionary<int, UpgradeInfo>();
    public Dictionary<int, UpgradeInfo> upgrades3 = new Dictionary<int, UpgradeInfo>();
    public Dictionary<int, UpgradeInfo> upgrades4 = new Dictionary<int, UpgradeInfo>();
    public Dictionary<int, UpgradeInfo> upgrades5 = new Dictionary<int, UpgradeInfo>();
    private HashSet<Tuple<int, int>> disallowedUpgrades = new HashSet<Tuple<int, int>>(); //add in format upgrade dictionary, index
    private static SerializableHashSet<int, int> permanentDisallowedUpgrades = new SerializableHashSet<int, int>();
    //end upgrades stuff

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Player player;

    public Action OnMapSize10; //not invoked but in tilemapper there is an if statement

    private ErrorHandler errorHandler;

    private static string difficulty = "basic";
    public static string skinPref = "normal";
    private int extraFood;
    private int segmentsPerGrow; //controlled in GameManager because player needs to use grow for other stuff as well

    public string SkinPref {
        get => skinPref;
        set => skinPref = value;
    }
    public string Difficulty {
        get => difficulty;
        set => difficulty = value;
    }
    private static int mapSize;
    public int MapSize {
        get => mapSize;
        set => mapSize = value;
    }

    private int mapSizeTemp;
    public int MapSizeTemp { //this is really 6 + MapSize, is changed on GM awake and player reset.
        get => mapSizeTemp;
        set => mapSizeTemp = value;
    }

    public void LoadData (GameData data) {
        segmentsPerGrow = data.segmentsPerGrow;
        extraFood = data.extraFood;
        mapSize = data.mapSize;
        permanentDisallowedUpgrades = data.permanentDisallowedUpgrades; //no need to save, these can be unlocked with meta currency in the main menu
        mapSizeTemp = mapSize + 6;
    }
    public void SaveData(GameData data) {
    }

    void Awake () { //sets map size, makes new instance, maybe gets skin preference
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1) { //if not on the title screen
            if (errorPanel != null) {
                errorHandler = errorPanel.GetComponent<ErrorHandler>();
            }
            for (int i = 0; i <= extraFood; i++) {
                Instantiate(Resources.Load("Prefabs/Food"));
            }
        } else {
            //anything that needs to be done in main menu
        }
        //difficulty = ? - not needed because it is static and set on the title screen
    }

    void Start () {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            Player.instance.OnReset += CheckMapSize;
            StartCoroutine(PostStart());
        }
    }

    private IEnumerator PostStart () {
        yield return new WaitForSecondsRealtime(.2f);
        Debug.Log("mapsize "+mapSize);
        mapSizeTemp = mapSize + 6;
        CheckMapSize();        
    }

    private void CheckMapSize() {
        if (mapSizeTemp >= 10) {
            OnMapSize10.Invoke();
        }
    }

    public void SetDictionaryValues () { //garter(common) is 0 (grey), python(uncommon) is 1 (green), rattlesnake(rare) is 2 (blue), gaboon viper(epic) is 3 (red shiny/royal purple), king cobra(legendary) is 4 (gold metallic/red), rainbow boa(mythic) is 5 (rainbow/aqua)
        upgrades0.Clear();
        upgrades1.Clear();
        upgrades2.Clear();
        upgrades3.Clear(); // how do I make this automated (be able to use a for loop to go through it) without having another layer I need to get
        upgrades4.Clear(); // to in order to access the UpgradeInfo values? is the way to group them without needing to go through that layer related to pointers?
        upgrades5.Clear();
        int i = 0; //garter \/
        upgrades0.Add(i, new UpgradeInfo("+1 Map Size", 0, 0)); i++; //MAKE SURE YOU DO NOT MISS AN INDEX OR START BELOW OR ABOVE 0
        upgrades0.Add(i, new UpgradeInfo("Slow Speed", 0, 0)); i++;   //I could fix that by doing .Keys.ElementAt(index); but that still would require index to be a valid key so actually I couldn't that's just how dictionaries work this whole comment is stupid
        upgrades0.Add(i, new UpgradeInfo("Remove 1 Segment", 0, 0)); i++; //index 4
        i = 0; //python \/
        upgrades1.Add(i, new UpgradeInfo("+10% XP", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("+2 Map Size", 0, 1)); i++; //index 3
        i = 0; //rattlesnake \/
        upgrades2.Add(i, new UpgradeInfo("Remove 3 Segments", 0, 2)); i++;
        upgrades2.Add(i, new UpgradeInfo("+1 Food", 0, 2)); i++;
        i = 0;//viper \/
        upgrades3.Add(i, new UpgradeInfo("+30 Seconds", 0, 3)); i++;
        i = 0;//cobra \/
        upgrades4.Add(i, new UpgradeInfo("Placeholder KC", 0, 4)); i++;
        i = 0;//boa \/
        upgrades5.Add(i, new UpgradeInfo("+3 Food", 0, 5)); i++; //index 0
    }

    private Dictionary<int, UpgradeInfo> GetUpgradeDictionaryByRarity(int rarity) {
        switch (rarity) {
            case 0: return upgrades0;
            case 1: return upgrades1;
            case 2: return upgrades2;
            case 3: return upgrades3;
            case 4: return upgrades4;
            case 5: return upgrades5;
            default: Debug.Log("Line 153 GameManager.cs rarity was not valid. rarity = "+rarity); return upgrades0;
        }
    }


    // this method should be called by the button which gets a signal from the player 
    //when the snakeScore gets to certain thresholds. these thresholds should be stored in an array in the 
    //player script so that they can be accessed and changed by different upgrades that the player acquires.

    //returns a new random upgrade, removes it from disallowed upgrades
    public UpgradeInfo ChooseRandomRunUpgrade(int rarity, ref int index) { 
        UpgradeInfo upgradeChosen;
        Dictionary<int, UpgradeInfo> selectedUpgrades = GetUpgradeDictionaryByRarity(rarity);
        upgradeChosen = ChooseUpgrade(selectedUpgrades, ref index, rarity);
        disallowedUpgrades.Add(new Tuple<int, int>(rarity, index));
        //Debug.Log("new disallowedUpgrade "+new Tuple<int, int>(rarity, index));
        return upgradeChosen;
    }
    
    //called in ChooseRandomRunUpgrade, returns a new random upgrade
    private UpgradeInfo ChooseUpgrade (Dictionary<int, UpgradeInfo> upgradeChoose, ref int index, int rarity) {
        List<int> validKeys = upgradeChoose.Keys.Where(key => !disallowedUpgrades.Contains(new Tuple<int, int>(rarity, key))
        && !permanentDisallowedUpgrades.Contains(new Tuple<int, int>(rarity, key))).ToList(); //don't really understand Linq expressions but saw it on StackOverflow
        index = UnityEngine.Random.Range(0, validKeys.Count);
        if (validKeys.Count > 0) {
            return upgradeChoose[validKeys[index]];
        } else {
            if (rarity !=0) {
                return ChooseUpgrade(GetUpgradeDictionaryByRarity(rarity - 1), ref index, rarity - 1); //using recursion!!! This I believe returns something that gives the correct name and rarity in runupgrades and then when used to actually upgrade uses the wrong index.
            } else {
                int randomKey = UnityEngine.Random.Range(0, upgradeChoose.Count);
                return upgradeChoose[randomKey];
            }
        }
    }

    public void AddPermanentDisallowedUpgrade(int rarity, int upgradeIndex) {
        permanentDisallowedUpgrades.Add(new Tuple<int, int>(rarity, upgradeIndex));
    }

    public void RemovePermanentDisallowedUpgrade(int rarity, int upgradeIndex) {
        permanentDisallowedUpgrades.Remove(new Tuple<int, int>(rarity, upgradeIndex));
    }

    private void IncreaseUpgradeLevel (UpgradeInfo currentUpgrade) {
        currentUpgrade.Level++;
        if (currentUpgrade.Rarity == 0) {
            GarterUpgrades(currentUpgrade);
        } else if (currentUpgrade.Rarity == 1) {
            PythonUpgrades(currentUpgrade);
        } else if (currentUpgrade.Rarity == 2) {
            RattlesnakeUpgrades(currentUpgrade);
        } else if (currentUpgrade.Rarity == 3) {
            ViperUpgrades(currentUpgrade);
        } else if (currentUpgrade.Rarity == 4) {
            CobraUpgrades(currentUpgrade);
        } else if (currentUpgrade.Rarity == 5) {
            BoaUpgrades(currentUpgrade);
        } else {
            Debug.Log("Rarity mismatch, rarity = "+currentUpgrade.Rarity);
        }
        if (mapSizeTemp >= 10) {
            OnMapSize10.Invoke();
        }
        //Debug.Log(currentUpgrade.Name+" increased to "+currentUpgrade.Level);
    }

    //in other program (button) call RunUpgrade
    public void RunUpgrade (UpgradeInfo upgrade, int index) {
        //Debug.Log(upgrade.Name+", rarity "+upgrade.Rarity); 
        //add one to the upgrade chosen
        IncreaseUpgradeLevel(upgrade);
        disallowedUpgrades.Clear();
    }

    //garter upgrades
    private void GarterUpgrades (UpgradeInfo currentUpgrade) { //class isn't passed by reference, the reference to the class is passed by value. Ask stratton about primitive types -> pointers to a location in memory, classes when given to methods are references to a class outside which values can be modified inside the method.
        player.Grow(segmentsPerGrow);
        switch (currentUpgrade.Name) {
            case "+1 Map Size":
                mapSizeTemp++;
                TileMapper.instance.RefreshTileMap();
                break;
            case "Slow Speed":
                //remove 2 player.DifficultyTime units from player time if that doesn't take player time below a little below .1f.
                for (int i = 0; i < 2; i++) {
                    if (player.LocalTimeScale > .08f + player.DifficultyTime) {
                        player.LocalTimeScale -= player.DifficultyTime;
                        //Debug.Log("subtracted "+player.DifficultyTime+ " from player localTimeScale.");
                    }
                }
                //Debug.Log("Player time scale = "+player.LocalTimeScale);
                break;
            case "Remove 1 Segment":
                player.RemoveSegment();
                player.RemoveSegment();
                break;
            default:
                //Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;

        }
    }

    //python upgrades
    private void PythonUpgrades (UpgradeInfo currentUpgrade) {
        player.Grow(segmentsPerGrow);
        switch (currentUpgrade.Name) {
            case "+2 Map Size":
                mapSizeTemp += 2;
                TileMapper.instance.RefreshTileMap();
                break;
            case "+10% XP":
                Debug.Log("player xp multi was "+Player.instance.XpMultiTemp);
                Player.instance.XpMultiTemp += .1f;
                Debug.Log("player xp multi now "+Player.instance.XpMultiTemp);
                break;
            default:
                //Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //rattlesnake upgrades
    private void RattlesnakeUpgrades (UpgradeInfo currentUpgrade) {
        player.Grow(segmentsPerGrow);
        switch (currentUpgrade.Name) {
            case "+1 Food":
                Instantiate(Resources.Load("Prefabs/TempFood"));
                break;
            case "Remove 3 Segments":
                for (int i = 0; i < 4; i++) {
                    player.RemoveSegment();
                }
                break;
            default:
                //Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //viper upgrades
    private void ViperUpgrades (UpgradeInfo currentUpgrade) {
        player.Grow(segmentsPerGrow);
        switch (currentUpgrade.Name) {
            case "+30 Seconds":
                RunTimer.instance.AddRunTime(30);
                break;
            default:
                //Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //cobra upgrades
    private void CobraUpgrades (UpgradeInfo currentUpgrade) {
        player.Grow(segmentsPerGrow);
        switch (currentUpgrade.Name) {
            case "+1 Map Size":
                mapSizeTemp++;
                TileMapper.instance.RefreshTileMap();
                break;
            default:
                //Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //boa upgrades
    private void BoaUpgrades (UpgradeInfo currentUpgrade) {
        //maybe boa upgrades make the player not grow as an added benefit to all of them.
        //either this or they only grow, don't speed up... Many thoughts ¯\_(ツ)_/¯
        player.DoneChoosing();
        switch (currentUpgrade.Name) {
            case "+3 Food":
                Instantiate(Resources.Load("Prefabs/TempFood"));
                Instantiate(Resources.Load("Prefabs/TempFood"));
                Instantiate(Resources.Load("Prefabs/TempFood"));
                break;
            default:
                //Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }
}