using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UI;

public class EverettUnlocker : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
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
