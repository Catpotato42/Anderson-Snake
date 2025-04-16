using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuPermaUpgrades : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private PlayFromAudioSource playAudio;
    private bool challengeUpgrades = false;
    public void LoadData (GameData data) {
        challengeUpgrades = data.challengeUpgrades;
        if (challengeUpgrades) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    public void SaveData (GameData data) {}

    void Awake () {
        playAudio = GetComponent<PlayFromAudioSource>();
    }
    public void OnPointerClick (PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            AudioManager.instance.PlayAudio("defaultButtonClick");
            SaveManager.instance.SaveGame();
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                SceneManager.LoadScene(4);
            } else if (SceneManager.GetActiveScene().buildIndex == 4) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
