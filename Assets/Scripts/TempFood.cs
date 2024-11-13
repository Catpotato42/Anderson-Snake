using UnityEngine;

public class TempFood : MonoBehaviour
{
    private GameObject playerObj;
    private Player player;
    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();        
        RandomPosition();
        player.OnReset += RemoveFood;
    }

    private void RandomPosition () {
        int x = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        int y = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        transform.position = new Vector2 (x, y);
    }

    private void RemoveFood () {
        player.OnReset -= RemoveFood;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!collider.CompareTag("Player") /*&& !collider.CompareTag("Enemy")*/) {
            return;
        } else {
            RandomPosition();
        }
    }
}
