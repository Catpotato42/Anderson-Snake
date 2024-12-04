using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EverettSkinEnabledOption : MonoBehaviour, IPointerClickHandler, ISaveManager
{

    private Button button;
    private string skinPref;
    private int highScoreEv;
    public void SaveData (GameData data) {
        data.skinPref = skinPref;
    }
    public void LoadData (GameData data) {
        skinPref = data.skinPref;
        highScoreEv = data.highScoreEv; //if high score everett is 50 or greater, you can use this skin.
    }
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (skinPref == "everett") {
            button.GetComponent<Image>().color = Color.green;
        } else {
            button.GetComponent<Image>().color = Color.red;
        }
        if (highScoreEv < 50 && skinPref != "everett") {
            button.GetComponent<Image>().color = Color.gray;
            gameObject.GetComponentInParent<TextMeshProUGUI>().text = "Git Gud";
        } else if (highScoreEv < 50 && skinPref == "everett") {
            Debug.Log("You shouldn't have that skin... (╯°□°）╯︵ ┻━┻"); //TODO: error message?
        }
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            if (skinPref == "normal" && highScoreEv >= 50) {
                skinPref = "everett";
                button.GetComponent<Image>().color = Color.green;
            } else if (skinPref == "everett") {
                skinPref = "normal";
                button.GetComponent<Image>().color = Color.red;
            } else if (skinPref == "normal" && highScoreEv < 50) {
                //nothing happens
            } else {
                Debug.Log("error: skinPref = "+skinPref);
            }
        }
    }
}
