using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EverettSkinEnabledOption : MonoBehaviour, IPointerClickHandler, ISaveManager
{

    private Button button;
    private string skinPref;
    private int highScoreEv;
    private bool hasEverettSkin;
    private bool acornFound;
    private bool canSpawnAcorn = true;
    [SerializeField] private GameObject goldAcorn;
    private AudioSource locked;
    public void SaveData(GameData data)
    {
        data.skinPref = skinPref;
    }
    public void LoadData (GameData data) {
        skinPref = data.skinPref;
        highScoreEv = data.highScoreEv; //if high score everett is 50 or greater, you can use this skin.
        hasEverettSkin = data.hasEverettSkin;
        acornFound = data.acorn4;
    }
    void Start()
    {
        locked = GetComponent<AudioSource>();
        button = gameObject.GetComponent<Button>();
        if (skinPref == "everett") {
            button.GetComponent<Image>().color = Color.green;
        } else {
            button.GetComponent<Image>().color = Color.red;
        }
        if (!hasEverettSkin && skinPref != "everett") {
            button.GetComponent<Image>().color = Color.gray;
            gameObject.GetComponentInParent<TextMeshProUGUI>().text = "Locked";
        }
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (skinPref == "normal" && hasEverettSkin) {
                skinPref = "everett";
                AudioManager.instance.PlayAudio("defaultButtonClick");
                button.GetComponent<Image>().color = Color.green;
                if (!acornFound && canSpawnAcorn) {
                    canSpawnAcorn = false;
                    Debug.Log("acornFound = "+acornFound+", spawning acorn");
                    GameObject acorn = Instantiate(goldAcorn, new Vector3(2.5f, 1.8f, 0f), Quaternion.identity);
                    acorn.transform.localScale = new Vector3 (2,2,0);
                    acorn.name = "Acorn4";
                    SaveManager.instance.UpdateSaveManagerObjects();
                    SaveManager.instance.LoadGame();
                }
            } else if (skinPref == "everett") {
                skinPref = "normal";
                AudioManager.instance.PlayAudio("defaultButtonClick");
                button.GetComponent<Image>().color = Color.red;
            } else if (skinPref == "normal" && !hasEverettSkin) {
                locked.Play();
                //nothing happens
            } else {
                Debug.Log("error: skinPref = "+skinPref);
            }
        }
    }
}
