using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuOptions : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            SaveManager.instance.SaveGame();
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                SceneManager.LoadScene(3);
            } else if (SceneManager.GetActiveScene().buildIndex == 3) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
