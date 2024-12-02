using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RunTimer : MonoBehaviour, ISaveManager
{
    public static RunTimer instance;
    private TextMeshProUGUI timerText;
    private float runTime;
    private float runTimeTracker;
    private Color darkRed = new Color(0.6176f, 0, 0);
    public void SaveData (GameData data) {
    }
    public void LoadData (GameData data) {
        runTime = data.runTime;
    }
    
    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
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

    public void AddRunTime(float seconds) {
        runTimeTracker+=seconds;
    }

    private void SetSeconds () {
        int minutes = (int)runTimeTracker / 60;
        int seconds = (int)runTimeTracker % 60;
        if (runTimeTracker >= 60f) { //if more than a minute on the runTimeTracker
            timerText.text = minutes + ":" + seconds.ToString("00");
        } else if (runTimeTracker < 60f && runTimeTracker > 10f){
            timerText.text = "0:"+ (int)(runTimeTracker);
        } else if (runTimeTracker < 10f) {
            if (timerText.alignment != TextAlignmentOptions.Left) {
                timerText.alignment = TextAlignmentOptions.Left;
            }
            timerText.text = "" + Mathf.Round(runTimeTracker*100f)/100f; //fixed the fucking formatting with single digits ಠ﹏ಠ
            if ((int)runTimeTracker % 2 != 0) { //odd numbers are red 
                timerText.color = darkRed;
                //play a sound?
            } else {
                timerText.color = Color.white;
            }
        }
    }
}
