using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    private SpriteRenderer sprite;
    void Awake() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        RandomPosition();
    }
    void Start () {
        Player.instance.OnReset += RandomPosition;
    }

    private void RandomPosition () {
        int x = Random.Range(-(GameManager.instance.MapSizeTemp / 2) + 1, GameManager.instance.MapSizeTemp / 2);
        int y = Random.Range(-(GameManager.instance.MapSizeTemp / 2) + 1, GameManager.instance.MapSizeTemp / 2);
        transform.position = new Vector2 (x, y);
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!collider.CompareTag("Player") && !collider.CompareTag("Enemy") && !collider.CompareTag("Obstacle")) {
            return;
        } else {
            RandomPosition();
        }
    }


}
