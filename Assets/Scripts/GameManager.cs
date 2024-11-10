using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System;
using UnityEngine.UIElements;

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
    [SerializeField] private TileMapper tileMap;
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
        mapSize = PlayerPrefs.GetInt("mapSize") + 6;
        if (SceneManager.GetActiveScene().buildIndex != 0) { //if not on the title screen
            if (errorPanel != null) {
            errorHandler = errorPanel.GetComponent<ErrorHandler>();
            }
        } else {
            PlayerPrefs.SetInt("extraSegments", 1);
            //reset all playerprefs here to what default values should be
            PlayerPrefs.SetInt("mapSize", 0);
        }
        SetDictionaryValues();
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

    void SetDictionaryValues () { //garter(common) is 0 (grey), python(uncommon) is 1 (green), rattlesnake(rare) is 2 (blue), gaboon viper(epic) is 3 (red shiny), king cobra(legendary) is 4 (gold metallic), rainbow boa(mythic) is 5 (rainbow)
        int i = 0;
        upgrades0.Add(i, new UpgradeInfo("mapSizeAdd1", 0, 0)); i++; //MAKE SURE YOU DO NOT MISS AN INDEX OR START BELOW OR ABOVE 0
        upgrades0.Add(i, new UpgradeInfo("speedSlow", 0, 0)); i++;   //I could fix that by doing .Keys.ElementAt(index); but that still would require index to be a valid key
        upgrades0.Add(i, new UpgradeInfo("damageAdd", 0, 0)); i++;
        upgrades0.Add(i, new UpgradeInfo("foodAdd", 0, 0)); i++;
        upgrades0.Add(i, new UpgradeInfo("removeSegment2", 0, 0)); i++; //index 4
        i = 0;
        upgrades1.Add(i, new UpgradeInfo("xpMore", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("damagePercentAdd", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("speedPercentSlow", 0, 1)); i++;
        upgrades1.Add(i, new UpgradeInfo("mapSizeAdd2", 0, 1)); i++; //index 3
        i = 0;
        upgrades2.Add(i, new UpgradeInfo("removeSegment4", 0, 2)); i++;
        upgrades2.Add(i, new UpgradeInfo("rattleSnakePlaceholder", 0, 2)); i++; //index 1
        i = 0;
        upgrades3.Add(i, new UpgradeInfo("fireSpeedAdd", 0, 3)); i++; //index 0, this should be a lot of fireSpeed
        i = 0;
        upgrades4.Add(i, new UpgradeInfo("projectileAdd1", 0, 4)); i++;
        upgrades4.Add(i, new UpgradeInfo("KingCobraPlaceholder", 0, 4)); i++; //index 1
        i = 0;
        upgrades5.Add(i, new UpgradeInfo("RainBowBoaPlaceholder", 0, 5)); i++; //index 0
        i = 0;
    }

    private Dictionary<int, UpgradeInfo> GetUpgradeDictionaryByRarity(int rarity) {
        switch (rarity) {
            case 0: return upgrades0;
            case 1: return upgrades1;
            case 2: return upgrades2;
            case 3: return upgrades3;
            case 4: return upgrades4;
            case 5: return upgrades5;
            default: return upgrades0;
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
                return ChooseUpgrade(GetUpgradeDictionaryByRarity(rarity - 1), ref index, rarity - 1); //using recursion!!!
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

    private void IncreaseUpgradeLevel (Dictionary<int, UpgradeInfo> dict, int index) {
        UpgradeInfo currentUpgrade = dict[index];
        currentUpgrade.Level++;
        if (currentUpgrade.Rarity == 0) {
            GarterUpgrades(currentUpgrade);
        }
        player.Grow();
        Debug.Log(dict[index].Name+" increased to "+dict[index].Level);
    }

    //in other program (button) call RunUpgrade
    public void RunUpgrade (UpgradeInfo upgrade, int index) {
        Debug.Log(upgrade.Name+", rarity "+upgrade.Rarity); //add one to the upgrade chosen
        Dictionary<int, UpgradeInfo> dict = GetUpgradeDictionaryByRarity(upgrade.Rarity);
        IncreaseUpgradeLevel(dict, index);
        player.UpgradeNumber++;
        disallowedUpgrades.Clear();
    }

    //garter upgrades
    private void GarterUpgrades (UpgradeInfo currentUpgrade) { //class isn't passed by reference, the reference to the class is passed by value. Ask stratton about primitive types -> pointers to a location in memory, classes when given to methods are references to a class outside which values can be modified inside the method. 
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                tileMap.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;

        }
    }

    //python upgrades
    private void PythonUpgrades (UpgradeInfo currentUpgrade) {
        switch (currentUpgrade.Name) {
            case "mapSizeAdd1":
                mapSize++;
                tileMap.RefreshTileMap();
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
                tileMap.RefreshTileMap();
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
                tileMap.RefreshTileMap();
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
                tileMap.RefreshTileMap();
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
                tileMap.RefreshTileMap();
                break;
            default:
                Debug.Log(currentUpgrade.Name+" hasn't been implemented yet.");
                break;
        }
    }
}