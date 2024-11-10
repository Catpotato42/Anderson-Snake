using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunUpgrades : MonoBehaviour, IPointerClickHandler
{
    private Player player;
    private GameManager.UpgradeInfo upgradeInfo;
    private GameObject choiceParent;
    private List<GameObject> choices = new List<GameObject>();
    private float python = 50, rattlesnake = 75, viper = 90, cobra = 95, boa = 97.5f; //may want to switch python and rattlesnake
    private int index = 0;
    void Start()
    {
        
        //TODO FOR UPGRADES: 
        //get meta upgrades
        //Done with base structure!
        choiceParent = GameObject.FindGameObjectWithTag("UpgradeOptions");
        int k = 0;
        while (k < choiceParent.transform.childCount) {
            Transform tempChoice = choiceParent.transform.GetChild(k);
            if (tempChoice.gameObject != gameObject){
                choices.Add(tempChoice.gameObject);
            }
            k++;
        }
        if (GameObject.FindGameObjectWithTag("Player") == null) {
            Debug.Log("bro there's no player on this screen"); //shouldn't be possible anyways but idk I should add some of these to get faster at writing them and get in the habit
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.OnUpgrade += InitButton;
        gameObject.SetActive(false);
    }

    private void InitButton (bool buttonOn) {
        gameObject.SetActive(buttonOn);
        float rarity = Random.Range(0, 1000)/10f;
        //Debug.Log("rarity generation: "+rarity);
        if (rarity < python && rarity >= 0) {
            rarity = 0; //garter
        } else if (rarity >= python && rarity < rattlesnake) {
            rarity = 1; //python
        } else if (rarity >= rattlesnake && rarity < viper) {
            rarity = 2;
        } else if (rarity >= viper && rarity < cobra) {
            rarity = 3;
        } else if (rarity >= cobra && rarity < boa) {
            rarity = 4;
        } else if (rarity >= boa && rarity < 100) {
            rarity = 5;
        } else {
            rarity = 0;
            Debug.Log("error: rarity out of bounds");
        }
        int finalRarity = (int)rarity;
        upgradeInfo = GameManager.instance.ChooseRandomRunUpgrade(finalRarity, ref index);
        displayUpgrade();
    }

    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            GameManager.instance.RunUpgrade(upgradeInfo, index);
            for (int i = 0; i < choices.Count; i++) {
                choices[i].GetComponent<RunUpgrades>().deactivateSelf();
            }
            gameObject.SetActive(false);
        }
    }

    public void deactivateSelf() { //needs to be public so the chosen upgrade can deactive other upgrade choices.
        gameObject.SetActive(false);
    }

    private void displayUpgrade () {
        GameObject titleUpgradeObj = gameObject.transform.GetChild(0).gameObject; //there should be two textmeshpros as the child
        TextMeshProUGUI titleUpgrade = titleUpgradeObj.GetComponent<TextMeshProUGUI>(); //INSTEAD I SHOULD JUST INSTANTIATE A PREFAB
        titleUpgrade.text = upgradeInfo.Name+", Level "+upgradeInfo.Level; //also need color based on rarity, more text objects
        Image buttonSprite = gameObject.GetComponent<Image>();
        switch(upgradeInfo.Rarity) {
            case 0:
                buttonSprite.color = new Color(.5f, .5f, .5f, 1);
                break;
            case 1:
                buttonSprite.color = new Color(50/255f, 138/255f, 73/255f, 1);
                break;
            case 2:
                buttonSprite.color = new Color(45/255, 85/255f, 227/255f, 1);
                break;
            case 3:
                buttonSprite.color = new Color(64/255f, 4/255f, 148/255f, 1);
                break;
            case 4:
                buttonSprite.color = new Color(199/255f, 14/255f, 14/255f, 1);
                break;
            case 5:
                buttonSprite.color = new Color(14/255f, 165/255f, 199/255f, 1);
                break;
            default:
                Debug.Log("RunUpgrades line 105, couldn't get the correct value for upgradeInfo.Rarity, returned "+upgradeInfo.Rarity+" instead.");
                buttonSprite.color = new Color(.05f, .05f, .05f, 1);
                break;
        }
    }

}
