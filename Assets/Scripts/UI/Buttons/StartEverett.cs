using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartEverett : MonoBehaviour, IPointerClickHandler
{
    void Start () {
        //Debug.Log("hasEverett exists: "+PlayerPrefs.HasKey("hasEverett"));
        if (PlayerPrefs.GetInt("hasEverett") == 1) {
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
