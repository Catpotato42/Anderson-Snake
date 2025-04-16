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
    private PlayUpgradeButtonAudio playUpgradeButtonAudio;
    public void SaveData (GameData data) {
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
        playUpgradeButtonAudio = GetComponent<PlayUpgradeButtonAudio>();
        button = gameObject.GetComponent<Button>();
        if (skinPref == "everett") {
            button.GetComponent<Image>().color = Color.green;
        } else {
            button.GetComponent<Image>().color = Color.red;
        }
        if (!hasEverettSkin && skinPref != "everett") {
            button.GetComponent<Image>().color = Color.gray;
            gameObject.GetComponentInParent<TextMeshProUGUI>().text = "Locked";
        } else if (highScoreEv < 50 && skinPref == "everett") {
            Debug.LogWarning("You shouldn't have that skin... (╯°□°）╯︵ ┻━┻");
        }
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (skinPref == "normal" && hasEverettSkin) {
                skinPref = "everett";
                playUpgradeButtonAudio.PlayChange();
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
                playUpgradeButtonAudio.PlayChange();
                button.GetComponent<Image>().color = Color.red;
            } else if (skinPref == "normal" && !hasEverettSkin) {
                playUpgradeButtonAudio.PlayLocked();
                //nothing happens
            } else {
                Debug.Log("error: skinPref = "+skinPref);
            }
        }
    }
}
