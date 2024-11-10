using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
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
    private HashSet<Tuple<int, int>> disallowedUpgrades = new HashSet<Tuple<int, int>>(); //add in format index
    private static HashSet<Tuple<int, int>> permanentDisallowedUpgrades = new HashSet<Tuple<int, int>>();
    //end upgrades stuff

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Player player;
    private GameObject highScoreObj;
    private ErrorHandler errorHandler;

    
    private static string skinPref = "basic";
    public string SkinPref {
        get => skinPref;
        set => skinPref = value;
    }
    private static int mapSize;

    public int MapSize {
        get => mapSize;
        set => mapSize = value;
    }

    void Awake () { //sets map size, makes new instance, maybe gets skin preference
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        if (SceneManager.GetActiveScene().buildIndex != 0) { //if not on the title screen
            if (errorPanel != null) {
                errorHandler = errorPanel.GetComponent<ErrorHandler>();
            }
            for (int i = 0; i <= PlayerPrefs.GetInt("extraFood"); i++) {
                Instantiate(Resources.Load("Prefabs/Food"));
            }
        } else {
            PlayerPrefs.SetInt("extraSegments", 1); //remove all the playerpref stuff later
            //reset all playerprefs here to what default values should be
            PlayerPrefs.SetInt("mapSize", 0);
        }
        //skinPref = ?
    }

    void Start () {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            SetHighScore();
            Camera mainCam = mainCamera.GetComponent<Camera>();
            mainCam.orthographicSize = mapSize - (mapSize/7f);
            mainCamera.transform.position = new Vector3(1, -.5f, -10);
        }
    }

    void SetHighScore () { //sets high score if object with tag high score is found
        if (GameObject.FindGameObjectWithTag("HighScore") != null) {
            highScoreObj = GameObject.FindGameObjectWithTag("HighScore");
            HighScore highScore = highScoreObj.GetComponent<HighScore>();
            if (skinPref == "everett") {
                highScore.updateHighScore("eHighScore");
            } else if (skinPref == "basic") {
                highScore.updateHighScore("HighScore");
            }
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
        upgrades0.Add(i, new UpgradeInfo("mapSizeAdd1", 0, 0)); i++; //MAKE SURE YOU DO NOT MISS AN INDEX OR START BELOW OR ABOVE 0
        upgrades0.Add(i, new UpgradeInfo("speedSlow", 0, 0)); i++;   //I could fix that by doing .Keys.ElementAt(index); but that still would require index to be a valid key so actually I couldn't that's just how dictionaries work this whole comment is stupid
        upgrades0.Add(i, new UpgradeInfo("damageAdd", 0, 0)); i++;
        upgrades0.Add(i, new UpgradeInfo("foodAdd", 0, 0)); i++;
        upgrades0.Add(i, new UpgradeInfo("removeSegment2", 0, 0)); i++; //index 4
        i = 0; //python \/
        upgrades1.Add(i, new UpgradeInfo("xpMore", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("damagePercentAdd", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("speedPercentSlow", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("mapSizeAdd2", 0, 1)); i++; //index 3
        i = 0; //rattlesnake \/
        upgrades2.Add(i, new UpgradeInfo("removeSegment4", 0, 2)); i++;
        upgrades2.Add(i, new UpgradeInfo("rattleSnakePlaceholder", 0, 2)); i++; //index 1
        i = 0;//viper \/
        upgrades3.Add(i, new UpgradeInfo("fireSpeedAdd", 0, 3)); i++; //index 0, this should be a lot of fireSpeed to make the rarity mean something
        upgrades3.Add(i, new UpgradeInfo("viperPlaceHolder", 0, 3)); i++; //so the non percent upgrades of higher rarities are still worth it long run
        i = 0;//cobra \/
        upgrades4.Add(i, new UpgradeInfo("projectileAdd1", 0, 4)); i++;
        upgrades4.Add(i, new UpgradeInfo("KingCobraPlaceholder", 0, 4)); i++; //index 1
        i = 0;//boa \/
        upgrades5.Add(i, new UpgradeInfo("RainBowBoaPlaceholder", 0, 5)); i++; //index 0
    }

    private Dictionary<int, UpgradeInfo> GetUpgradeDictionaryByRarity(int rarity) {
        switch (rarity) {
            case 0: return upgrades0;
            case 1: return upgrades1;
            case 2: return upgrades2;
            case 3: return upgrades3;
            case 4: return upgrades4;
            case 5: return upgrades5;
            default: Debug.Log("Line 152 GameManager.cs rarity was not valid. rarity = "+rarity); return upgrades0;
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
        player.Grow();
        Debug.Log(currentUpgrade.Name+" increased to "+currentUpgrade.Level);
    }

    //in other program (button) call RunUpgrade
    public void RunUpgrade (UpgradeInfo upgrade, int index) {
        Debug.Log(upgrade.Name+", rarity "+upgrade.Rarity); //add one to the upgrade chosen
        IncreaseUpgradeLevel(upgrade);
        disallowedUpgrades.Clear();
    }

    //garter upgrades
    private void GarterUpgrades (UpgradeInfo currentUpgrade) { //class isn't passed by reference, the reference to the class is passed by value. Ask stratton about primitive types -> pointers to a location in memory, classes when given to methods are references to a class outside which values can be modified inside the method. 
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                TileMapper.instance.RefreshTileMap();
                break;
            case "foodAdd":
                Instantiate(Resources.Load("Prefabs/TempFood"));
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;

        }
    }

    //python upgrades
    private void PythonUpgrades (UpgradeInfo currentUpgrade) {
        switch (currentUpgrade.Name) {
            case "mapSizeAdd2":
                mapSize += 2;
                TileMapper.instance.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //rattlesnake upgrades
    private void RattlesnakeUpgrades (UpgradeInfo currentUpgrade) {
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                TileMapper.instance.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //viper upgrades
    private void ViperUpgrades (UpgradeInfo currentUpgrade) {
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                TileMapper.instance.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //cobra upgrades
    private void CobraUpgrades (UpgradeInfo currentUpgrade) {
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                TileMapper.instance.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }

    //boa upgrades
    private void BoaUpgrades (UpgradeInfo currentUpgrade) {
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                TileMapper.instance.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }
}