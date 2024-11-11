using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool cameraDetached = false;
    [SerializeField] private Player player;
    void Awake() {
        GameManager.instance.OnMapSize15 += DetachCamera;
    }

    private void DetachCamera () {
        cameraDetached = true;
    }

    void Update () {
        detachedChecker();
    }

    private void detachedChecker () {
        if (cameraDetached) {
            transform.position = player.transform.position + new Vector3(0, 1, -10);
        }
    }
}
