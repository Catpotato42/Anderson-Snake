using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject playerObj; //make the spawner be at like 1000, 1000, 0 or something so that it doesn't spawn on the player then move.
    private Player player;
    private float health;
    void Awake() {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
        player.OnReset += RemoveEnemy;
        if (gameObject.CompareTag("Enemy1")) {
            health = 100;
        } else {
            Debug.Log("Enemy tag = "+gameObject.tag);
        }
    }


    private void RemoveEnemy () {
        player.OnReset -= RemoveEnemy;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Food") && collision.gameObject.GetComponent<Enemy>() == null) {
            UniversalBulletScript bulletScript = collision.gameObject.GetComponent<UniversalBulletScript>();
            health -= bulletScript.Damage;
            if (health <= 0) {
                RemoveEnemy();
            }
        }
    }

}
