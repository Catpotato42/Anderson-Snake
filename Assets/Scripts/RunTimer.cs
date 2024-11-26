using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RunTimer : MonoBehaviour, ISaveManager
{
    private TextMeshProUGUI timerText;
    private float runTime;
    private float runTimeTracker;
    public void SaveData (GameData data) {
    }
    public void LoadData (GameData data) {
        runTime = data.runTime;
    }
    
    void Awake()
    {
        timerText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Start () {
        Player.instance.OnReset += ResetTimer;
    }

    void Update()
    {
        DecrementTimer();
    }

    private void ResetTimer () { //ask stratton about better way to put a 0 in front of seconds if single digits
        runTimeTracker = runTime;
        timerText.alignment = TextAlignmentOptions.Center;
        SetSeconds();
    }

    private void DecrementTimer () {
        runTimeTracker -= Time.deltaTime;
        SetSeconds();
        if (runTimeTracker <= 0) {
            Player.instance.KillPlayer();
            ResetTimer();
        }
    }

    private void SetSeconds () {
        int minutes = (int)runTimeTracker / 60;
        int seconds = (int)runTimeTracker % 60;
        float tenSecs = Mathf.Round(runTimeTracker*100f)/100f;
        if (runTimeTracker >= 60f) { //if more than a minute on the runTimeTracker
            timerText.text = minutes + ":" + seconds.ToString("00");
        } else if (runTimeTracker < 60f && runTimeTracker > 10f){
            timerText.text = "0:"+ (int)(runTimeTracker);
        } else {
            if (timerText.alignment != TextAlignmentOptions.Left) {
                timerText.alignment = TextAlignmentOptions.Left;
            }
            timerText.text = "" + Mathf.Round(runTimeTracker*100f)/100f; //TODO: fix the fucking formatting with single digits ಠ﹏ಠ
        }
    }
}
