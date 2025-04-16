using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SummonAcorn : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool acornFound;
    private bool canSpawnAcorn = true;
    [SerializeField] private GameObject goldAcorn;
    public void SaveData (GameData data) {}
    public void LoadData (GameData data) {
        acornFound = data.acorn5;
    }
    void Start()
    {
        if (acornFound) {
            gameObject.SetActive(false);
        }
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (!acornFound && canSpawnAcorn) {
                canSpawnAcorn = false;
                Debug.Log("acornFound = "+acornFound+", spawning acorn");
                GameObject acorn = Instantiate(goldAcorn, new Vector3(2.3f, -2.7f, 0f), Quaternion.identity);
                acorn.transform.localScale = new Vector3 (2,2,0);
                acorn.name = "Acorn5";
                SaveManager.instance.UpdateSaveManagerObjects();
                SaveManager.instance.LoadGame();
            }
        }
    }
}
