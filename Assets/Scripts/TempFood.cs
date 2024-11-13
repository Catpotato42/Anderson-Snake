using UnityEngine;

public class TempFood : MonoBehaviour
{
    void Awake()
    {    
        RandomPosition();
        Player.instance.OnReset += RemoveFood;
    }

    private void RandomPosition () {
        int x = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        int y = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        transform.position = new Vector2 (x, y);
    }

    private void RemoveFood () {
        Player.instance.OnReset -= RemoveFood;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!collider.CompareTag("Player") && !collider.CompareTag("Enemy")) {
            return;
        } else {
            RandomPosition();
        }
    }
}
