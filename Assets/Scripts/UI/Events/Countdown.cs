using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    // READY... GO
    [SerializeField] Player player;
    [SerializeField] HighScore highScore;

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
        yield return new WaitForSecondsRealtime(.02f);
        highScore.UpdateHighScore(GameManager.instance.Difficulty); // put here so Start in SaveManager has time to run and update values
        yield return new WaitForSecondsRealtime(.18f);
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
