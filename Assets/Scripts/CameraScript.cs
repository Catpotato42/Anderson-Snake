using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool cameraDetached = false;
    private float smoothSpeed = 0.125f;
    [SerializeField] private Player player;
    void Start() {
        cameraDetached = false;
        if (GameManager.instance.MapSize >= 10) { //if playerprefs mapSize + GameManager MapSize >= 15
            DetachCamera();
        }
        GameManager.instance.OnMapSize10 += DetachCamera; //this could probably be changed to just set cameraDetached to true, maybe todo but it's fine as is.
    }

    private void DetachCamera () {
        cameraDetached = true;
    }

    void Update () {
        detachedChecker();
    }

    private void detachedChecker () {
        if (cameraDetached) {
            Vector3 playerPos = player.transform.position + new Vector3(0, 1, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, playerPos, smoothSpeed * player.LocalTimeScale);
            transform.position = smoothedPosition;
        }
    }
}