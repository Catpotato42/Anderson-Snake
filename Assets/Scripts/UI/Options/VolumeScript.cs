using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour, ISaveManager
{
    private Slider volumeBar;
    private float tempVolume; //used to store volume before we can change the slider value (as SaveManager is 100 ms before Start in Script Execution order)

    public void LoadData (GameData data) {
        tempVolume = data.volume;
    }
    public void SaveData (GameData data) {
        data.volume = volumeBar.value;
    }

    void Start () {
        volumeBar = GetComponent<Slider>();
        volumeBar.value = tempVolume;
        volumeBar.onValueChanged.AddListener(delegate {OnValueChange();});
    }

    private void OnValueChange () {
        AudioManager.instance.PlayAudio("defaultButtonClick");
        SaveManager.instance.SaveGame();
        SaveManager.instance.LoadGame();
    }
}
