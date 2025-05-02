using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcornUnlocker : MonoBehaviour
{
    void Start()
    {
        UnlockerChangeState(false, "nothing");
    }

    private void UnlockerChangeState (bool stateChange, string diff) {
        TextMeshPro textrenderer = gameObject.GetComponent<TextMeshPro>();
        textrenderer.text = diff;
        textrenderer.enabled = stateChange;
    }

    public void RunAcornCollected (string amount) {
        Debug.Log(amount);
        StartCoroutine(DiffUnlocked(amount));
    }

    private IEnumerator DiffUnlocked (string diff) {
        for (int i = 0; i < 6; i++) {
            UnlockerChangeState(true, diff);
            yield return new WaitForSecondsRealtime(.7f);
            UnlockerChangeState(false, diff);
            yield return new WaitForSecondsRealtime(.3f);
        }
    }
}
