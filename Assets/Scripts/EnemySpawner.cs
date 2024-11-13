using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    private class WaveInfo {
        public string EnemyType;
        public int Amount;
        public float Interval;
        public WaveInfo(string enemyType, int amount, float interval) {
            EnemyType = enemyType;
            Amount = amount;
            Interval = interval;
        }
    }
    private Dictionary<int, WaveInfo> waves0 = new Dictionary<int, WaveInfo>();
    private Dictionary<int, WaveInfo> waves1 = new Dictionary<int, WaveInfo>();
    private Dictionary<int, WaveInfo> waves2 = new Dictionary<int, WaveInfo>();
    private List<Vector3> disallowed;
    private int wavesTracker = 0;
    private float wavetimeTracker = 0;
    [SerializeField] private GameObject enemy0; //stationary
    [SerializeField] private GameObject enemy1; //moves toward you
    [SerializeField] private GameObject enemy2; //shoots at you
    [SerializeField] private GameObject enemy3;
    [SerializeField] private Dictionary<string, GameObject> enemyList;
    private GameObject playerObj;
    private Player player;
    void Start() {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
        player.OnReset += ResetWaves;
        InitWaveValues(GameManager.instance.Difficulty);
        enemyList = new Dictionary<string, GameObject>{{"enemy0", enemy0}, {"enemy1", enemy1}, {"enemy2", enemy2}, {"enemy3", enemy3}};
    }

    private void InitWaveValues (string difficulty) {
        if (difficulty == "basic") { //doing this differently from upgradeNumber because that was confusing af.
            for (int i = 0; i < 2; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 1, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
            for (int i = 2; i < 3; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 5, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
            for (int i = 3; i < 5; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 7, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
            for (int i = 5; i < 8; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 8, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
        } else if (difficulty == "everett") {
            for (int i = 0; i < 2; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 2, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
            for (int i = 2; i < 3; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 5, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
            for (int i = 3; i < 5; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 7, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
            for (int i = 5; i < 8; i++) {
                waves0.Add(i, new WaveInfo("enemy0", 8, 30));
                waves1.Add(i, new WaveInfo("none", 0, 0));
                waves2.Add(i, new WaveInfo("none", 0, 0));
            }
        }
    }
    void Update() {
        wavetimeTracker += Time.deltaTime;
        if (Mathf.Round(wavetimeTracker) == 5 || Mathf.Round(wavetimeTracker) == 25) {
            Debug.Log("waveTimeTracker = "+wavetimeTracker);
        }
        if (wavetimeTracker >= waves0[wavesTracker].Interval) {
            wavetimeTracker = 0;
            for (int i = 0; i < waves0[wavesTracker].Amount; i++) {
                Instantiate(enemyList[waves0[wavesTracker].EnemyType]);
            } 
            for (int i = 0; i < waves1[wavesTracker].Amount; i++) {
                Instantiate(enemyList[waves1[wavesTracker].EnemyType]);
            }
            for (int i = 0; i < waves2[wavesTracker].Amount; i++) {
                Instantiate(enemyList[waves2[wavesTracker].EnemyType]);
            }
            if (waves0[wavesTracker+1] == null) {
                waves0.Add(wavesTracker+1, new WaveInfo("enemy0", 5, 30));
                waves1.Add(wavesTracker+1, new WaveInfo("none", 0, 0));
                waves2.Add(wavesTracker+1, new WaveInfo("none", 0, 0));
            }
            wavesTracker++;
        }
    }
    private Vector3 RandomPosition () {
        int x = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        int y = Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2);
        Vector3 xy = new Vector3 (x, y, 0);
        int size = player.segments.Count;
        DisallowedPositions(xy, size);
        disallowed.Add(xy);
        return xy;
    }

    private void DisallowedPositions (Vector3 newPos, int size) { //this is a search, but the positions are not sorted which means we cannot be more efficient (i mean maybe we can but not that much more), and it's not worth it to sort as n should never be > ~10
        if (size > disallowed.Count) {
            for (int i = 0; i < size; i++) {
                if (player.segments[i].transform.position == newPos || disallowed[i] == newPos) {
                    RandomPosition();
                    return;
                }
            }
        } else {
            for (int i = 0; i < disallowed.Count; i++) {
                if (player.segments[i].transform.position == newPos || disallowed[i] == newPos) {
                    RandomPosition();
                    return;
                }
            }
        }
    }

    private void ResetWaves () {
        wavesTracker = 0;
        wavetimeTracker = 0;
    }
}
