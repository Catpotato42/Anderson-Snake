using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour {
    //I did the saving through a tutorial, so it's generally a bit cleaner than the other code. Some things to note:
    //I want to be able to save while playing the actual game so that the current highScore and score + meta curr is not lost.
    [Header("File Storage Config (disabled)")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private GameData gameData;
    public static SaveManager instance {get; private set;}
    private List<ISaveManager> saveManagerObjects;
    private FileDataHandler dataHandler;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        fileName = "ASnakeData";
        useEncryption = false;
    }

    void Start () {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption); //docs for Application.persistentDataPath
        this.saveManagerObjects = FindAllSaveManagerObjects();
        LoadGame();
    }

    public void NewGame () {
        gameData = new GameData();
        gameData.timerDone = false;
    }

    public void NewGamePlus (bool done) { //starts a new game while retaining high scores.
        int highScoreB = gameData.highScoreB;
        int highScoreM = gameData.highScoreM;
        int highScoreH = gameData.highScoreH;
        int highScoreEv = gameData.highScoreEv;
        bool acorn1 = gameData.acorn1;
        bool acorn2 = gameData.acorn2;
        bool acorn3 = gameData.acorn3;
        bool acorn4 = gameData.acorn4;
        bool acorn5 = gameData.acorn5;
        bool allAcornsCollected = gameData.allAcornsCollected;
        int acornsCollected = gameData.acornsCollected;
        gameData = new GameData();
        gameData.acorn1 = acorn1;
        gameData.acorn2 = acorn2;
        gameData.acorn3 = acorn3;
        gameData.acorn4 = acorn4;
        gameData.acorn5 = acorn5;
        gameData.allAcornsCollected = allAcornsCollected;
        gameData.acornsCollected = acornsCollected;
        gameData.timerDone = done;
        gameData.highScoreB = highScoreB;
        gameData.highScoreM = highScoreM;
        gameData.highScoreH = highScoreH;
        gameData.highScoreEv = highScoreEv;
        gameData.hasChallengeRun = true;
        if (!done) {
            gameData.challengeUpgrades = true;
        } else {
            gameData.challengeUpgrades = false;
        }
        dataHandler.Save(gameData); //should serialize the updated gameData to json and save to file without being affected by outside scripts
        LoadGame(); //may need a SaveGame before this
    }

    public void LoadGame () {
        //load saved file or make a new game
        gameData = dataHandler.Load();

        if (gameData == null) {
            Debug.Log("Game not found, making default new game.");
            NewGame();
        }
        //push loaded data
        foreach (ISaveManager saveManagerObj in saveManagerObjects) {
            if (saveManagerObj != null) {
                saveManagerObj.LoadData(gameData);
            } else {
                Debug.Log("Missing Save Manager Object!");
            }
        }

        //Debug.Log("Loaded mapSize and health, "+gameData.mapSize+" and "+gameData.extraHealth+".");
    }

    public void SaveGame () { //should be called when going back and forth from the main menu as well as in OnApplicationQuit
        //pass data to other scripts
        foreach (ISaveManager saveManagerObj in saveManagerObjects) {
            if (saveManagerObj != null)
            {
                saveManagerObj.SaveData(gameData);
            }
        }
        //Debug.Log("Loaded mapSize and health, "+gameData.mapSize+" and "+gameData.extraHealth+".");
        //save to file using the data handler
        dataHandler.Save(gameData);
    }

    private List<ISaveManager> FindAllSaveManagerObjects () {
        IEnumerable<ISaveManager> saveManagerObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagerObjects);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    public void UpdateSaveManagerObjects () {
        saveManagerObjects = FindAllSaveManagerObjects();
    }
}
