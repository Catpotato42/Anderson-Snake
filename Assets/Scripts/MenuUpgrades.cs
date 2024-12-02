using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuUpgrades : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            SaveManager.instance.SaveGame();
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                SceneManager.LoadScene(2);
            } else if (SceneManager.GetActiveScene().buildIndex == 2) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
