using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuOptions : MonoBehaviour, IPointerClickHandler
{
    private PlayFromAudioSource playAudio;
    void Awake () {
        playAudio = GetComponent<PlayFromAudioSource>();
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            ButtonClicked();
        }
    }

    private void ButtonClicked() {
        SaveManager.instance.SaveGame();
        AudioManager.instance.PlayAudio("defaultButtonClick");
        if (SceneManager.GetActiveScene().buildIndex == 0) { //should replace with the name, build index isn't safe. Maybe next game though.
            SceneManager.LoadScene(3);
        } else if (SceneManager.GetActiveScene().buildIndex == 3) {
            SceneManager.LoadScene(0);
        }
    }
}