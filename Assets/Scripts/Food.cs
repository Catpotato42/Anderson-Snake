using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
        RandomPosition();
        player.OnReset += RandomPosition;
    }

    private void RandomPosition () {
        int x = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        int y = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        transform.position = new Vector2 (x, y);
    }

    void OnTriggerEnter2D () {
        RandomPosition();
    }
}
