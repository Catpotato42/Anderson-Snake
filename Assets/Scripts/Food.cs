using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RandomPosition();
    }

    void RandomPosition () {
        int x = Random.Range(-16, 16);
        int y = Random.Range(-16, 16);
        transform.position = new Vector2 (x + .475f, y + .515f);
    }

    void OnTriggerEnter2D () {
        RandomPosition();
    }
}
