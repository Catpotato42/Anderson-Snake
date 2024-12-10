using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTracker : MonoBehaviour, ISaveManager
{
    [SerializeField] private string type;
    private bool isEnabled;
    private System.Func<float> tracker; //c# equivalent to a pointer
    private Slider cdBar;
    public void LoadData (GameData data) {
        switch (type) {
            case "Dash":
                isEnabled = data.hasDash;
                break;
            case "TimeSlow":
                isEnabled = data.hasTimeSlow;
                break;
            case "Reverse":
                isEnabled = data.hasReverse;
                break;
            default:
                Debug.Log("type invalid - LoadData (CooldownTracker). type = "+type);
                break;
        }
    }
    public void SaveData (GameData data) {
    }
    void Start() {
        if (!isEnabled) {
            Destroy(gameObject);
        }
        cdBar = GetComponent<Slider>();
        switch (type) {
            case "Dash":
                cdBar.maxValue = Player.instance.DashCooldown;
                tracker = () => Player.instance.DashCooldownTracker;
                break;
            case "TimeSlow":
                cdBar.maxValue = Player.instance.TimeSlowCooldown;
                tracker = () => Player.instance.TimeSlowCooldownTracker;
                break;
            case "Reverse":
                cdBar.maxValue = Player.instance.ReverseCooldown;
                tracker = () => Player.instance.ReverseCooldownTracker;
                break;
            default:
                Debug.Log("type invalid - LoadData (CooldownTracker). type = "+type);
                break;
        }
    }

    void Update () {
        cdBar.value = tracker();
    }
}
