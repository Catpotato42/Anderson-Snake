using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq.Expressions;

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
    public static Dictionary<int, UpgradeInfo> upgrades0 = new Dictionary<int, UpgradeInfo>();
    public static Dictionary<int, UpgradeInfo> upgrades1 = new Dictionary<int, UpgradeInfo>();
    public static Dictionary<int, UpgradeInfo> upgrades2 = new Dictionary<int, UpgradeInfo>();
    public static Dictionary<int, UpgradeInfo> upgrades3 = new Dictionary<int, UpgradeInfo>();
    public static Dictionary<int, UpgradeInfo> upgrades4 = new Dictionary<int, UpgradeInfo>();
    public static Dictionary<int, UpgradeInfo> upgrades5 = new Dictionary<int, UpgradeInfo>();
    private Dictionary<int, int> disallowedUpgrades = new Dictionary<int, int>(); //add in format rarity, index
    //end upgrades stuff

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject errorPanel;
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
        upgrades0.Add(i, new UpgradeInfo("mapSizeAdd1", 0, 0)); i++;
        upgrades0.Add(i, new UpgradeInfo("speedSlow", 0, 0)); i++;
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
        upgrades2.Add(i, new UpgradeInfo("rattleSnakePlaceholder", 0, 2)); i++;
        upgrades2.Add(i, new UpgradeInfo("rattleSnakePlaceholder", 0, 2)); i++;
        upgrades2.Add(i, new UpgradeInfo("rattleSnakePlaceholder", 0, 2)); i++;
        upgrades2.Add(i, new UpgradeInfo("rattleSnakePlaceholder", 0, 2)); i++; //index 4
        i = 0;
        upgrades3.Add(i, new UpgradeInfo("fireSpeedAdd", 0, 3)); i++; //index 0, this should be a lot of fireSpeed
        i = 0;
        upgrades4.Add(i, new UpgradeInfo("projectileAdd1", 0, 4)); i++;
        upgrades4.Add(i, new UpgradeInfo("KingCobraPlaceholder", 0, 4)); i++; //index 1
        i = 0;
        upgrades5.Add(i, new UpgradeInfo("RainBowBoaPlaceholder", 0, 5)); i++; //index 0
        i = 0;
    }


    // this method should be called by the button which gets a signal from the player 
    //when the snakeScore gets to certain thresholds. these thresholds should be stored in an array in the 
    //player script so that they can be accessed and changed by different upgrades that the player acquires.
    public UpgradeInfo ChooseRandomRunUpgrade(int rarity) { 
        UpgradeInfo upgradeChosen;
        int index;
        switch (rarity) {
            case 0:
                index = Random.Range(0, upgrades0.Count);
                while (index == disallowedUpgrades[0]) {
                    index = Random.Range(0, upgrades0.Count);
                }
                upgradeChosen = upgrades0[index];
                break;
            case 1:
                index = Random.Range(0, upgrades1.Count);
                while (index == disallowedUpgrades[1]) {
                    index = Random.Range(0, upgrades1.Count);
                }
                upgradeChosen = upgrades1[index];
                break;
            case 2:
                index = Random.Range(0, upgrades2.Count);
                while (index == disallowedUpgrades[2]) {
                    index = Random.Range(0, upgrades2.Count);
                }
                upgradeChosen = upgrades2[index];
                break;
            case 3:
                index = Random.Range(0, upgrades3.Count);
                while (index == disallowedUpgrades[3]) {
                    index = Random.Range(0, upgrades3.Count);
                }
                upgradeChosen = upgrades3[index];
                break;
            case 4:
                index = Random.Range(0, upgrades4.Count);
                while (index == disallowedUpgrades[4]) {
                    index = Random.Range(0, upgrades4.Count);
                }
                upgradeChosen = upgrades4[index];
                break;
            case 5:
                index = Random.Range(0, upgrades5.Count);
                while (index == disallowedUpgrades[5]) {
                    index = Random.Range(0, upgrades5.Count);
                }
                upgradeChosen = upgrades5[index];
                break;
            default:
                index = Random.Range(0, upgrades0.Count);
                while (index == disallowedUpgrades[0]) {
                    index = Random.Range(0, upgrades0.Count);
                }
                upgradeChosen = upgrades0[index];
                Debug.Log("No rarity/ invalid rarity specified");
                break;
        }
        disallowedUpgrades.Add(rarity, index);
        return upgradeChosen;
    }
    //in other program (buttons) call RunUpgrade
    public void RunUpgrade (string upgrade) {

    }
}
