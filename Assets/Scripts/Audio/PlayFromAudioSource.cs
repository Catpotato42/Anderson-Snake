using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayFromAudioSource : MonoBehaviour
{
    //this is actually only used for buttons that change the scene.
    private AudioSource audioSource;
    void Awake()
    {
        AudioSource tempAudio = GetComponent<AudioSource>();
        if (tempAudio != null) {
            audioSource = tempAudio;
        } else {
            Debug.Log("item "+gameObject.name+" does not have an audio source component attached!");
        }
    }

    public void PlayAudio () {
        if (audioSource != null) {
            audioSource.Play();
        }
    }

    public IEnumerator DestroyAfterLoad () {
        if (transform.parent != null) {
            transform.SetParent(null, true); //orphans child but keeps position
        }
        DontDestroyOnLoad(gameObject); //applies to even for example the reset button. this will not cause any problems down the line.
        yield return new WaitForSecondsRealtime(.1f); //jank jank jank jank jank
        Destroy(gameObject);
    }
}
