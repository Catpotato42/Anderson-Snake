using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    private GameObject playerObj;
    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Player player = playerObj.GetComponent<Player>();   
        RandomPosition();
        player.OnReset += RandomPosition;
    }

    private void RandomPosition () {
        int x = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        int y = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        transform.position = new Vector2 (x, y);
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!collider.CompareTag("Player") /*&& !collider.CompareTag("Enemy1")*/) {
            return;
        } else {
            RandomPosition();
        }
    }
}
