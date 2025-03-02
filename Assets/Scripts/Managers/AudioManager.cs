using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip defaultButtonClick;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip death;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); //note: this means that all scenes need a fully loaded audiomanager if you want to be able to start the game from any scene.
        CheckForParent();
        audioSource = GetComponent<AudioSource>();
    }

    private void CheckForParent () {
        GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (parentPlayer != null) {
            gameObject.transform.SetParent(parentPlayer.transform);
        }
    }

    public void PlayAudio (String clipName) {
        switch(clipName) {
            case "defaultButtonClick":
                audioSource.PlayOneShot(defaultButtonClick);
                break;
            case "hurt":
                audioSource.PlayOneShot(hurt);
                break;
            case "death":
                audioSource.PlayOneShot(death);
                break;
            default:
                Debug.Log("In PlayAudio: clip name "+clipName+" does not match known clips");
                break;
        }
    }
}
