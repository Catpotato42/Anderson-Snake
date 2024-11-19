using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour {
    //I did the saving through a tutorial, so it's generally a bit cleaner than the other code. Some things to note:
    //I want to be able to save while playing the actual game so that the current highScore and score + meta curr is not lost.
    private GameData gameData;
    public SaveManager instance {get; private set;}
    private List<ISaveManager> saveManagerObjects;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start () {
        this.saveManagerObjects = FindAllSaveManagerObjects();
        LoadGame();
    }

    public void NewGame () {
        gameData = new GameData(); 
    }

    public void LoadGame () {
        //load saved file or make a new game
        if (gameData == null) {
            Debug.Log("Game not found, making default new game.");
            NewGame();
        }
        //push loaded data
        foreach (ISaveManager saveManagerObj in saveManagerObjects) {
            saveManagerObj.LoadData(gameData);
        }

        Debug.Log("Loaded mapSize and health, "+gameData.mapSize+" and "+gameData.extraHealth+".");
    }

    public void SaveGame () { //should be called when going back to the main menu as well
        //pass data to other scripts
        foreach (ISaveManager saveManagerObj in saveManagerObjects) {
            saveManagerObj.SaveData(gameData);
        }
        Debug.Log("Loaded mapSize and health, "+gameData.mapSize+" and "+gameData.extraHealth+".");
        //save to file
    }

    private List<ISaveManager> FindAllSaveManagerObjects () {
        IEnumerable<ISaveManager> saveManagerObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagerObjects);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }
}
