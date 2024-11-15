using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool cameraDetached = false;
    private float smoothSpeed = 0.125f;
    [SerializeField] private Player player;
    void Start() {
        cameraDetached = false;
        gameObject.transform.position = new Vector3 (1, -.5f, -10); //maybe redundant because player calls OnReset but probably not as onreset is called in start also
        if (GameManager.instance.MapSize >= 10) { //if playerprefs mapSize + GameManager MapSize >= 15
            DetachCamera();
        }
        GameManager.instance.OnMapSize10 += DetachCamera; //this could probably be changed to just set cameraDetached to true, maybe todo but it's fine as is.
        player.OnReset += AttachCamera;
    }

    private void DetachCamera () {
        Debug.Log("Detached camera");
        cameraDetached = true;
    }
    
    private void AttachCamera () {
        if (GameManager.instance.MapSize < 10) {
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