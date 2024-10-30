using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            GameManager.instance.SkinPref = "basic";
            GameManager.instance.MapSize = 8;
            SceneManager.LoadScene(1);
        }
    }
}
