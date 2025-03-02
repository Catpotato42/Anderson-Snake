using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour, ISaveManager
{
    public static SpeedrunTimer instance; //not for access just for 1 per scene
    private float speedrunTimer;
    private bool timerDone;
    private TextMeshProUGUI speedrunText;
    public void LoadData (GameData data) {
        speedrunTimer = data.speedrunTimer;
        timerDone = data.timerDone;
    }
    public void SaveData (GameData data) {
        data.speedrunTimer = speedrunTimer;
    }
    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        speedrunText = GetComponent<TextMeshProUGUI>();
    }

    void Update ()
    {
        if (Player.instance != null) {
            timerDone = Player.instance.timerDone;
        }
        if (!timerDone) {
            speedrunTimer += Time.unscaledDeltaTime;
        }
        SetSeconds();
    }

    private void SetSeconds () {//copied completely from RunTimer
        int minutes = (int)speedrunTimer / 60;
        int seconds = (int)speedrunTimer % 60;
        if (speedrunTimer >= 60f) { //if more than a minute on the speedrunTimer
            speedrunText.text = minutes + ":" + seconds.ToString("00");
        } else if (speedrunTimer < 60f && speedrunTimer > 10f){
            speedrunText.text = "0:"+ (int)speedrunTimer;
        } else if (speedrunTimer < 10f) {
            /*if (speedrunText.alignment != TextAlignmentOptions.Left) {
                speedrunText.alignment = TextAlignmentOptions.Left;
            }*/
            speedrunText.text = "" + Mathf.Round(speedrunTimer*100f)/100f; //fixed the fucking formatting with single digits ಠ﹏ಠ
        }
    }
}
