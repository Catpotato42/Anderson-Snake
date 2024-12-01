using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyUnlocker : MonoBehaviour
{
    void Start()
    {
        UnlockerChangeState(false, "nothing");
        Player.instance.OnDiffUnlock += RunDiffUnlock;
    }

    private void UnlockerChangeState (bool stateChange, string diff) {
        TextMeshProUGUI textrenderer = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        Image renderer = gameObject.GetComponent<Image>();
        textrenderer.text = diff + " MODE UNLOCKED!!!";
        textrenderer.enabled = stateChange;
        renderer.enabled = stateChange;
    }

    private void RunDiffUnlock (string diff) {
        StartCoroutine(DiffUnlocked(diff));
    }

    private IEnumerator DiffUnlocked (string diff) {
        for (int i = 0; i < 6; i++) {
            UnlockerChangeState(true, diff);
            yield return new WaitForSecondsRealtime(.3f);
            UnlockerChangeState(false, diff);
            yield return new WaitForSecondsRealtime(.3f);
        }
    }
}
