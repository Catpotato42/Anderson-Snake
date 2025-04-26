using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewRunScript : MonoBehaviour, ISaveManager, IPointerClickHandler
{
    //this should ONLY be on the title screen.
    private bool timerDone;
    private bool hasChallengeRun;
    public void LoadData (GameData data) {
        timerDone = data.timerDone;
        hasChallengeRun = data.hasChallengeRun;
        gameObject.SetActive(hasChallengeRun);
    }
    public void SaveData (GameData data) {
        data.timerDone = timerDone;
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (timerDone) { //if the timer has been stopped or if it was not started yet
            SaveManager.instance.NewGamePlus(false);
            AudioManager.instance.PlayAudio("defaultButtonClick");
            timerDone = false;
        } else { //otherwise, reset timer and start new game.
            SaveManager.instance.NewGamePlus(true);
            AudioManager.instance.PlayAudio("defaultButtonClick");
            timerDone = true;
        }
    }
}
