using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, ISaveManager
{
    public static AudioManager instance;
    private AudioSource audioSource;
    private float volume;
    [SerializeField] private AudioClip defaultButtonClick;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip goldAcorn;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.Log("Destroying AudioManager");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); //note: this means that all scenes need a fully loaded audiomanager if you want to be able to start the game from any scene.
        CheckForParent();
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadData (GameData data) {
        if (this == null || audioSource == null) { //This is only needed when we have an AudioManager prefab in this scene, as that prefab is destroyed but still referenced.
            //Even without this if statement the code works as the instance that isn't deleted is also called, but this stops warning messages from unity.
            return;
        }
        audioSource = GetComponent<AudioSource>();
        volume = data.volume;
        audioSource.volume = volume; //done on start as well but this means it is changed on every save, so if it changes during a scene
    }
    public void SaveData (GameData data) {}

    void Start () {
        audioSource.volume = volume;
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
            case "eat":
                if (eat != null) {
                    audioSource.PlayOneShot(eat);
                } else {
                    Debug.Log("eat not found in PlayAudio!");
                }
                break;
            case "goldAcorn":
                audioSource.PlayOneShot(goldAcorn);
                break;
            default:
                Debug.Log("In PlayAudio: clip name "+clipName+" does not match known clips");
                break;
        }
    }
}
