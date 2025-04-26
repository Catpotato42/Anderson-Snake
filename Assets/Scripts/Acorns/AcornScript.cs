using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AcornScript : MonoBehaviour, ISaveManager
{
    private bool thisAcornCollected = false;
    private bool extraSave = false;
    private bool growing = false;
    private float smoothTime = .5f;
    private float targetSize = 0;
    private float velocity = 0f;
    public void LoadData (GameData data) {
        if (this == null)
        {
            return;
        }
        switch (name)
        {
            case "Acorn1":
                thisAcornCollected = data.acorn1;
                break;
            case "Acorn2":
                thisAcornCollected = data.acorn2;
                break;
            case "Acorn3":
                thisAcornCollected = data.acorn3;
                break;
            case "Acorn4":
                thisAcornCollected = data.acorn4;
                break;
            case "Acorn5":
                thisAcornCollected = data.acorn5;
                break;
            default:
                Debug.Log("AcornScript on non-acorn, name = " + name);
                break;
        }
        if (thisAcornCollected) {
            gameObject.SetActive(false);
        }
    }
    public void SaveData (GameData data) {
        if (this == null)
        {
            return;
        }
        switch (name)
        {
            case "Acorn1":
                if (data.acorn1 == true && thisAcornCollected == true)
                {
                    extraSave = true;
                }
                data.acorn1 = thisAcornCollected;
                Debug.Log("Acorn1 Collected");
                break;
            case "Acorn2":
                if (data.acorn2 == true && thisAcornCollected == true)
                {
                    extraSave = true;
                }
                data.acorn2 = thisAcornCollected;
                Debug.Log("Acorn2 Collected");
                break;
            case "Acorn3":
                if (data.acorn3 == true && thisAcornCollected == true)
                {
                    extraSave = true;
                }
                data.acorn3 = thisAcornCollected;
                Debug.Log("Acorn3 Collected");
                break;
            case "Acorn4":
                if (data.acorn4 == true && thisAcornCollected == true)
                {
                    extraSave = true;
                }
                data.acorn4 = thisAcornCollected;
                Debug.Log("Acorn4 Collected");
                break;
            case "Acorn5":
                if (data.acorn5 == true && thisAcornCollected == true)
                {
                    extraSave = true;
                }
                data.acorn5 = thisAcornCollected;
                Debug.Log("Acorn5 Collected");
                break;
            default:
                Debug.Log("AcornScript on non-acorn, name = " + name);
                break;
        }
        if (thisAcornCollected && !extraSave) {
            data.acornsCollected++;
            Debug.Log("Acorns Collected: "+data.acornsCollected);
        }
        if (data.acornsCollected >= 5) {
            Debug.Log("ALL ACORNS COLLECTED! Number of Acorns collected (should be 5): "+data.acornsCollected);
            data.allAcornsCollected = true;
            //TODO: Steam achievement
        }
    }
    private void OnMouseDown()
    {
        if (thisAcornCollected) {
            return;
        }
        Debug.Log("OnMouseDown triggered "+name);
        AudioManager.instance.PlayAudio("eat");
        thisAcornCollected = true;
        SaveManager.instance.SaveGame();
        StartCoroutine(removeAcorn());
    }
    
    private IEnumerator removeAcorn () {
        growing = true;
        yield return new WaitForSeconds(smoothTime); //TODO: replace with animation
        gameObject.SetActive(false);
    }

    void Start () {
        targetSize = 2*transform.localScale.x;
    }

    void Update () {
        if (growing) {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, targetSize, ref velocity, smoothTime);
            transform.localScale = new Vector3(newScale,newScale,0);
        }
    }
}
