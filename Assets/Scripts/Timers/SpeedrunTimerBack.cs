using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunTimerBack : MonoBehaviour, ISaveManager
{
    private bool hasChallengeRun;
    public void LoadData(GameData data) {
        hasChallengeRun = data.hasChallengeRun;
        gameObject.SetActive(hasChallengeRun);
    }
    public void SaveData(GameData data) {}
}