using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartEverett : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private bool hasEverett = false;
    public void LoadData (GameData data) {
        this.hasEverett = data.hasEverett;
        if (hasEverett) {
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
            GameManager.instance.Difficulty = "everett";
            AudioManager.instance.PlayAudio("defaultButtonClick");
            SaveManager.instance.SaveGame();
            SceneManager.LoadScene(1);
        }
    }
}
