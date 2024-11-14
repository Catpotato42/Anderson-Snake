using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            GameManager.instance.Difficulty = "basic";
            //PlayerPrefs.SetInt("mapSize", 0);
            SceneManager.LoadScene(1);
        }
    }
}
