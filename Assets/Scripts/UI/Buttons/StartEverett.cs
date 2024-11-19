using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartEverett : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasEverett = false;
    public void LoadData (GameData data) {
        this.hasEverett = data.hasEverett;
    }
    public void SaveData (GameData data) {
        return;
    }
    void Start () {
        //Debug.Log("hasEverett exists: "+PlayerPrefs.HasKey("hasEverett"));
        if (hasEverett) {
            gameObject.SetActive(true);
            //Debug.Log("active");
        } else {
            gameObject.SetActive(false);
        }
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            //Debug.Log("clicked");
            GameManager.instance.Difficulty = "everett";
            //PlayerPrefs.SetInt("mapSize", 8); //placeholder till you can buy map size 
            SceneManager.LoadScene(1);
        }
    }
}
