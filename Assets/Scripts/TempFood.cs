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
        Destroy(gameObject);
    }

    void OnTriggerEnter2D () {
        RandomPosition();
    }
}
