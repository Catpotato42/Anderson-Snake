using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornScript : MonoBehaviour, ISaveManager
{
    private bool thisAcornCollected = false;
    private int acornCollected = 0;
    public void LoadData (GameData data) {
        switch (name) {
            case "Acorn1":
                thisAcornCollected = data.acorn1;
                break;
            case "Acorn2":
                thisAcornCollected = data.acorn2;
                break;
            case "Acorn3":
                thisAcornCollected = data.acorn3;
                break;
            case "Acorn4":
                thisAcornCollected = data.acorn4;
                break;
            case "Acorn5":
                thisAcornCollected = data.acorn5;
                break;
            default:
                Debug.Log("AcornScript on non-acorn, name = "+name);
                break;
        }
    }
    public void SaveData (GameData data) {
        if (thisAcornCollected) {
            data.acornsCollected += acornCollected;
        }
        if (data.acornsCollected >= 5) {
            data.allAcornsCollected = true;
            //TODO: Steam achievement
        }
        switch (name) {
            case "Acorn1":
                data.acorn1 = thisAcornCollected;
                break;
            case "Acorn2":
                data.acorn2 = thisAcornCollected;
                break;
            case "Acorn3":
                data.acorn3 = thisAcornCollected;
                break;
            case "Acorn4":
                data.acorn4 = thisAcornCollected;
                break;
            case "Acorn5":
                data.acorn5 = thisAcornCollected;
                break;
            default:
                Debug.Log("AcornScript on non-acorn, name = "+name);
                break;
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown triggered "+name);
        AudioManager.instance.PlayAudio("eat");
        thisAcornCollected = true;
        acornCollected = 1;
        SaveManager.instance.SaveGame();
        gameObject.SetActive(false);
    }
    void Start()
    {
        if (thisAcornCollected) {
            gameObject.SetActive(false);
        }
    }
}
