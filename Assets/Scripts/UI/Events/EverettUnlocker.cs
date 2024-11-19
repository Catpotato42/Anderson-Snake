using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UI;

public class EverettUnlocker : MonoBehaviour, ISaveManager
{
    [SerializeField] private GameObject player;
    private bool hasMedium = false;
    private bool hasHard = false;
    private bool hasEverett = false;

    // Start is called before the first frame update
    public void SaveData (GameData data) {
        data.hasEverett = hasEverett;
    }
    public void LoadData (GameData data) {
        hasEverett = data.hasEverett;
    }
    void Start()
    {
        EverettChangeState(false);
        Player playerScript = player.GetComponent<Player>();
        playerScript.OnEverettUnlock += RunEverettUnlock;
    }

    private void EverettChangeState (bool stateChange) {
        TextMeshProUGUI textrenderer = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        Image renderer = gameObject.GetComponent<Image>();
        textrenderer.enabled = stateChange;
        renderer.enabled = stateChange;
    }

    private void RunEverettUnlock (bool unlocked) {
        StartCoroutine(EverettUnlocked());
        hasEverett = true;
    }

    private IEnumerator EverettUnlocked () {
        for (int i = 0; i < 6; i++) {
            EverettChangeState(true);
            yield return new WaitForSecondsRealtime(.3f);
            EverettChangeState(false);
            yield return new WaitForSecondsRealtime(.3f);
        }
    }
}
