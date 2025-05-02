using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour, IPointerClickHandler
{
    private AudioSource audioSource;
    void Awake () {
        AudioSource tempAudio = GetComponent<AudioSource>();
        if (tempAudio != null) {
            audioSource = tempAudio;
        } else {
            Debug.Log("item "+gameObject.name+" does not have an audio source component attached!");
        }
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (audioSource != null) {
                AudioManager.instance.PlayAudio("defaultButtonClick");
                Debug.Log("Played restart clip");
            }
            Player.instance.ResetSnake();
        }
    }
}
