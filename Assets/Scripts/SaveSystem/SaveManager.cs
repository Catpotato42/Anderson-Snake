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
    }

    public void NewGamePlus (bool done) { //starts a new game while retaining high scores.
        int highScoreB = gameData.highScoreB;
        int highScoreM = gameData.highScoreM;
        int highScoreH = gameData.highScoreH;
        int highScoreEv = gameData.highScoreEv;
        gameData = new GameData();
        gameData.timerDone = done;
        gameData.highScoreB = highScoreB;
        gameData.highScoreM = highScoreM;
        gameData.highScoreH = highScoreH;
        gameData.highScoreEv = highScoreEv;
        dataHandler.Save(gameData); //should serialize the updated gameData to json and save to file without being affected by outside scripts
        LoadGame(); //may need a SaveGame before this
    }

    public void LoadGame () {
        //load saved file or make a new game
        this.gameData = dataHandler.Load();

        if (gameData == null) {
            Debug.Log("Game not found, making default new game.");
            NewGame();
        }
        //push loaded data
        foreach (ISaveManager saveManagerObj in saveManagerObjects) {
            saveManagerObj.LoadData(gameData);
        }

        //Debug.Log("Loaded mapSize and health, "+gameData.mapSize+" and "+gameData.extraHealth+".");
    }

    public void SaveGame () { //should be called when going back and forth from the main menu as well as in OnApplicationQuit
        //pass data to other scripts
        foreach (ISaveManager saveManagerObj in saveManagerObjects) {
            saveManagerObj.SaveData(gameData);
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
}
