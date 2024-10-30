using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject errorPanel;
    private GameObject highScoreObj;
    private ErrorHandler errorHandler;

    private string eightByEight = "Prefabs/Background8x8";
    private string sixteenBySixteen = "Prefabs/Background16x16";
    
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
    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        if (errorPanel != null) {
            errorHandler = errorPanel.GetComponent<ErrorHandler>();
        }
        //skinPref = ?
    }

    void Start () {
        setHighScore();
        Camera mainCam = mainCamera.GetComponent<Camera>();
        if (mapSize == 0) {
            mapSize = 8;
        }
        if (mapSize == 16) {
            mainCam.orthographicSize = 11;
        } else if (mapSize == 8) {
            mainCam.orthographicSize = 6;
        } else {
            mainCam.orthographicSize = 5;
        }
        SetMapSize();
    }

    void setHighScore () { //sets high score if object with tag high score is found
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

    void SetMapSize () {
        Scene currentScene = SceneManager.GetActiveScene(); //important because if I want different scenes with like bosses this may need to change
        if (currentScene.name != "Title screen") {
            if (mapSize == 8) {
                TryMap(eightByEight);
            } else if (mapSize == 16) {
                TryMap(sixteenBySixteen);
            } else {
                TryMap(eightByEight, 8);
            }
        }
    }

    void TryMap (string mapLocation) {
        if (Resources.Load<GameObject>(mapLocation) != null) {
            Instantiate(Resources.Load<GameObject>(mapLocation));
        } else {
            StartCoroutine(errorHandler.ShowError(mapLocation + " not found! Check your files to see if this map is missing."));
        }
    }
    void TryMap (string mapLocation, int additionalError) {
        if (Resources.Load<GameObject>(mapLocation) != null) {
            Instantiate(Resources.Load<GameObject>(mapLocation));
            StartCoroutine(errorHandler.ShowError("No map found, loading " + mapLocation));
        } else {
            StartCoroutine(errorHandler.ShowError(mapLocation + " not found! Check your files to see if this map is missing, and probably redownload the game too cause you really shouldn't get this error. Error "+additionalError+"."));
        }
    }
}
