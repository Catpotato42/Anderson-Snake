using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        mapSize = PlayerPrefs.GetInt("mapSize");
        if (PlayerPrefs.GetInt("mapSize") == 0) {
            PlayerPrefs.SetInt("mapSize", 8);
            mapSize = 8;
        }
        //Debug.Log("Got to after mapsize set but before Scene loaded");
        SetMapSize();
    }

    void SetMapSize () {
        Scene currentScene = SceneManager.GetActiveScene();
        //Debug.Log("in setmapsize");
        if (currentScene.name != "Title screen") {
            if (mapSize == 8) {
                Instantiate(Resources.Load<GameObject>("Prefabs/background8x8"));
                //Debug.Log("Instantiated");
            } else {
                Debug.Log("Missing Map Error");
                Instantiate(Resources.Load<GameObject>("Prefabs/background8x8"));
            }
        }
    }
}
