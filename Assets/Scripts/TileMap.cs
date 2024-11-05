using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    [SerializeField] private Tilemap floorMap, obstacleMap;
    [SerializeField] private TileBase floorTile, l, lup, ld, r, rup, rd, up, d;
    public static TileMap instance;
    void Start() {
        RefreshTileMap();
    }
    
    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void RefreshTileMap() { //makes the whole map
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "TitleScreen") {
            return;
        }
        int width = GameManager.instance.MapSize + 2, height = GameManager.instance.MapSize + 2;
        Vector3Int centerRef = new Vector3Int(-width / 2, -height / 2, 0);

        for (int y = 0; y < height; y++) { //goes down columns 
            for (int x = 0; x < width; x++) {
                if (x != 0 && x != (width - 1) && y != 0 && y != (height - 1)) { //PLEASE MAKE THIS A SWITCH CASE FUTURE ME
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    floorMap.SetTile(floorPosition, floorTile);
                } else if (x == 0 && y == 0) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, lup);
                } else if (x == 0 && y != 0 && y != (height - 1)) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, l);
                } else if (x == 0 && y == (height - 1)) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, ld);
                } else if (x == (width - 1)&& y == (height - 1)) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, rup);
                } else if (x == (width - 1) && y != 0 && y != (height - 1)) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, r);
                } else if (x == (width - 1) && y == 0) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, rd);
                } else if (x != 0 && x != (width - 1) && y == 0) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, up);
                } else if (x != 0 && x != (width - 1) && y == (height - 1)) {
                    Vector3Int floorPosition = new Vector3Int (x, y, 0);
                    obstacleMap.SetTile(floorPosition, d);
                }
            }
        }
        //Debug.Log($"Center Position: {centerRef.x}, {centerRef.y}");
        floorMap.transform.position = new Vector3(centerRef.x + .5f, centerRef.y -.5f, 0);
        obstacleMap.transform.position = new Vector3(centerRef.x + .5f, centerRef.y - .5f, 0);
    }
}
