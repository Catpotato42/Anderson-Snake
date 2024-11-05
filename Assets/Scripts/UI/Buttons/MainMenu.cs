using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            PlayerPrefs.SetInt("mapSize", 0); //placeholder for resetting all temporary playerPrefs (upgrades)
            SceneManager.LoadScene(0);
        }
    }
}
