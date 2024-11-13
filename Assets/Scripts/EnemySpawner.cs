using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private class waveInfo {
        public string EnemyType;
        public int Amount;
        public float Interval;
        waveInfo(string enemyType, int amount, float interval) {
            EnemyType = enemyType;
            Amount = amount;
            Interval = interval;
        }
    }
    private Dictionary<int, waveInfo> waves0 = new Dictionary<int, waveInfo>();
    private Dictionary<int, waveInfo> waves1= new Dictionary<int, waveInfo>();
    private int wavesTracker = 0;
    private float wavetimeTracker = 0;
    [SerializeField] private GameObject enemy0;
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    [SerializeField] private Dictionary<string, GameObject> enemyList;
    private GameObject playerObj;
    private Player player;
    void Awake() {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
        player.OnReset += ResetWaves;
        InitWaveValues(GameManager.instance.Difficulty);
        enemyList = new Dictionary<string, GameObject>{{"enemy0", enemy0}, {"enemy1", enemy1}, {"enemy2", enemy2}, {"enemy3", enemy3}};
    }

    private void InitWaveValues (string difficulty) {
        if (difficulty == "basic") {
            for (int i = 0; i < 10; i++) { //doing this differently from upgradeNumber because that was confusing af.
                
            }
        }
    }
    void Update() {
        wavetimeTracker += Time.deltaTime;
        if (wavetimeTracker >= waves0[wavesTracker].Interval) {
            for (int i = 0; i < waves0[wavesTracker].Amount; i++) {
                Instantiate(enemyList[waves0[wavesTracker].EnemyType]);
            }
            wavesTracker++;
            wavetimeTracker = 0;
        }
    }
    private void ResetWaves () {
        wavesTracker = 0;
        wavetimeTracker = 0;
    }
}
