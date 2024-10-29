using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartEverett : MonoBehaviour, IPointerClickHandler
{
    void Start () {
        //Debug.Log("scoreEverett exists: "+PlayerPrefs.HasKey("scoreEverett"));
        if (PlayerPrefs.GetInt("scoreEverett") == 1) {
            gameObject.SetActive(true);
            //Debug.Log("active");
        } else {
            gameObject.SetActive(false);
        }
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            //Debug.Log("clicked");
            GameManager.instance.SkinPref = "everett";
            SceneManager.LoadScene(1);
        }
    }
}
