using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMedium : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasMedium = false;
    public void LoadData (GameData data) {
        this.hasMedium = data.hasMedium;
        if (hasMedium) {
            gameObject.SetActive(true);
            //Debug.Log("active");
        } else {
            gameObject.SetActive(false);
        }
    }
    public void SaveData (GameData data) {
    }
    private PlayFromAudioSource playAudio;
    void Awake () {
        playAudio = GetComponent<PlayFromAudioSource>();
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            //Debug.Log("clicked");
            GameManager.instance.Difficulty = "medium";
            AudioManager.instance.PlayAudio("defaultButtonClick");
            SaveManager.instance.SaveGame();
            SceneManager.LoadScene(1);
        }
    }
}
