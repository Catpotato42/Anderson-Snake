using UnityEngine;

public class PlayUpgradeButtonAudio : MonoBehaviour
{
    //Defunct as far as I'm aware, TODO: delete script
    private AudioSource[] audioSources;
    private AudioSource change;
    private AudioSource locked;
    void Start () {
        audioSources = GetComponents<AudioSource>();
        foreach (AudioSource temp in audioSources) {
            if (temp.clip.name != "Locked") { //it is != Locked because then we can have buttons without the locked audio sound needed
                change = temp;
            } else {
                locked = temp;
            }
        }
    }

    public void PlayChange () {
        change.Play();
    }

    public void PlayLocked () {
        locked.Play();
    }
}
