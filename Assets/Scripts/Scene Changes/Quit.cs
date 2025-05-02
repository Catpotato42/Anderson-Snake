using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quit : MonoBehaviour, IPointerClickHandler
{
    private PlayFromAudioSource playAudio;
    void Awake () {
        playAudio = GetComponent<PlayFromAudioSource>();
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            ButtonClicked();
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            ButtonClicked();
        }
    }

    private void ButtonClicked() {
        Debug.Log("quitting");
        SaveManager.instance.SaveGame();
        AudioManager.instance.PlayAudio("defaultButtonClick");
        Application.Quit();
    }
}
