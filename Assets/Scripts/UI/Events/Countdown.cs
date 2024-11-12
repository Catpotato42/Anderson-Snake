using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    // READY.(.)(.) GO
    [SerializeField] Player player;
    void Awake () {
        player.OnReset += CountdownStart;
    }
    private void CountdownStart() {
        gameObject.SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine () {
        var Countdown = gameObject.GetComponent<TextMeshProUGUI>();
        Time.timeScale = 0f;
        Countdown.text = "Ready.";
        yield return new WaitForSecondsRealtime(.2f);
        Countdown.text = "Ready..";
        yield return new WaitForSecondsRealtime(.2f);
        Countdown.text = "Ready...";
        yield return new WaitForSecondsRealtime(.2f);
        Countdown.text = "GO";
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(.8f);
        Countdown.text = "";
        gameObject.SetActive(false);
    }
}
