using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool cameraDetached = false;
    [SerializeField] private float smoothSpeed = 0.15f;
    [SerializeField] private Player player;
    void Start() {
        cameraDetached = false;
        gameObject.transform.position = new Vector3 (1, -.5f, -10); //maybe redundant because player calls OnReset but probably not as onreset is called in start also
        if (GameManager.instance.MapSizeTemp >= 10) { //if MapSize + GameManager MapSizeTemp >= 16
            DetachCamera();
        }
        GameManager.instance.OnMapSize10 += DetachCamera;
        player.OnReset += AttachCamera;
    }

    private void DetachCamera () {
        Debug.Log("Detached camera");
        cameraDetached = true;
    }
    
    private void AttachCamera () {
        if (GameManager.instance.MapSizeTemp < 10) {
            cameraDetached = false;
            gameObject.transform.position = new Vector3 (1, -.5f, -10);
        }
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