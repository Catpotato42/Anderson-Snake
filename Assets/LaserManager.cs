using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserManager : MonoBehaviour
{
    private float timeTracker = 0;
    private int laserWaveTracker = 0;
    [SerializeField] private GameObject laserWarning;
    [SerializeField] private GameObject laser;
    private LaserWaves laserWave;
    private List<int> lt;

    private class LaserWaves {
        public int Amount;
        public float WaveTime;
        public float SpawnTime;
        public bool Vertical;
        public LaserWaves (int amount, float waveTime, float spawnTime, bool vertical) {
            Amount = amount;
            WaveTime = waveTime;
            SpawnTime = spawnTime;
            Vertical = vertical;
        }
    }

    void Awake() {
        InitializeLaserWaves();
    }

    void Start () {
        Player.instance.OnReset += InitializeLaserWaves;
        SetLt(GameManager.instance.Difficulty);
    }

    private void SetLt (string difficulty) {
        if (difficulty == "basic") {
            lt = new List<int> {5, 12, 18, 30};
        } else if (difficulty == "medium") {
            lt = new List<int> {5, 12, 18, 30};
        } else if (difficulty == "hard") {
            lt = new List<int> {5, 12, 18, 30};
        } else if (difficulty == "everett") {
            lt = new List<int> {5, 12, 18, 30};
        }
    }

    private void InitializeLaserWaves () {
        timeTracker = 0;
        laserWaveTracker = 0;
        laserWave = new LaserWaves(1, 15, 3, true);
    }

    void Update () {
        timeTracker += Time.deltaTime;
        if (timeTracker >= laserWave.WaveTime) {
            //Debug.Log(" laser timeTracker = "+timeTracker+", going to spawn new wave "+laserWaveTracker);
            //reset tracker
            timeTracker = 0;
            laserWaveTracker++;
            //Debug.Log("Laser past tracker sets");
            //do wave logic
            WaveSpawning(laserWave);
            //Debug.Log(laserWave.Amount+" lasers should've been spawned.");
            //set next wave
            //these values should be set depending on how many waves have passed
            NextLaserWave();
        }
    }

    private void NextLaserWave() {
        int nextAmount = 1;
        float nextWaveTime = 2;
        float nextSpawnTime = 3;
        bool vertical = true; //set as these also for debugging purposes.
        if (laserWaveTracker < lt[0]) {
            nextAmount = Random.Range(1,3); //max exclusive
            nextWaveTime = Random.Range(10, 20);
            nextSpawnTime = 2;
            vertical = false;
        } else if (laserWaveTracker >= lt[0] && laserWaveTracker < lt[1]) {
            nextAmount = Random.Range(2,5); //max exclusive
            nextWaveTime = Random.Range(10, 20);
            nextSpawnTime = 2;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        } else if (laserWaveTracker >= lt[1] && laserWaveTracker < lt[2]) {
            nextAmount = Random.Range(4,7); //max exclusive
            nextWaveTime = Random.Range(8, 18);
            nextSpawnTime = 1.5f;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        } else if (laserWaveTracker >= lt[2] && laserWaveTracker < lt[3]) {
            nextAmount = Random.Range(6,11); //max exclusive
            nextWaveTime = Random.Range(8, 18);
            nextSpawnTime = 1.5f;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        } else if (laserWaveTracker >= lt[3]){
            nextAmount = Random.Range(10,15); //max exclusive
            nextWaveTime = Random.Range(6, 16);
            nextSpawnTime = 1.5f;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        }
        if (laserWaveTracker >= 30 && (GameManager.instance.Difficulty == "hard" || GameManager.instance.Difficulty == "everett")) {
            nextAmount = Random.Range(12,17); //max exclusive
            nextWaveTime = Random.Range(4, 16);
            nextSpawnTime = 1f;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        }
        //then set laserWave to a new Tuple containing these.
        laserWave = new LaserWaves(nextAmount, nextWaveTime, nextSpawnTime, vertical);
    }

    private void WaveSpawning (LaserWaves currWave) {
        //Debug.Log("'Hacker voice': I'm in IEnumerator WaveSpawning");
        for (int i = 0; i < currWave.Amount; i++) {
            StartCoroutine(LocationValues(currWave));
        }
    }

    private System.Collections.IEnumerator LocationValues (LaserWaves currWave) {
        //waits a random interval between warning laser spawns to make it look visually interesting, added onto amount of time to wait to spawn the real laser in coroutine
        float randomTime = Random.Range(0, 51)/100f; //max exclusive
        //Debug.Log("boutta wait for "+randomTime+" seconds this IEnum boring ah hell");
        yield return new WaitForSeconds(randomTime);
        //Debug.Log("Ever watched Oppenheimer that's what writing these debugs feels like. I'm also only writing to myself cause this stupid ass fucking code won't ");
        float odd;
        if ((GameManager.instance.MapSize % 2) == 0) {
            odd = .5f;
        } else {
            odd = 1;
        }
        Vector3 location;
        if (currWave.Vertical) {
            location = new Vector3(Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2), odd - 1, 0);
        } else {
            location = new Vector3(odd, Random.Range(-(GameManager.instance.MapSize / 2) + 1, GameManager.instance.MapSize / 2), 0);
        }
        Debug.Log("location: "+location);
        GameObject warningObject = Instantiate(laserWarning, location, Quaternion.identity);
        if (currWave.Vertical) {
            warningObject.transform.localScale = new Vector2(1, GameManager.instance.MapSize);
        } else {
            warningObject.transform.localScale = new Vector2(GameManager.instance.MapSize, 1);
        }
        StartCoroutine(SpawnLaser(currWave.SpawnTime, randomTime, currWave.Vertical, location, warningObject));
    }

    private System.Collections.IEnumerator SpawnLaser (float waitTime, float extraTime, bool vertical, Vector3 location, GameObject warning) {
        //Debug.Log("in IEnum SpawnLaser, waitTime = "+waitTime+", extraTime (randomTime) = "+extraTime);
        yield return new WaitForSeconds(waitTime + extraTime);
        GameObject laserRef  = Instantiate(laser, location, Quaternion.identity);
        if (vertical) {
            laserRef.transform.localScale = new Vector2(1, GameManager.instance.MapSize);
        } else {
            laserRef.transform.localScale = new Vector2(GameManager.instance.MapSize, 1);
        }
        //Debug.Log("laser location x= "+laserRef.transform.position.x+", laser location y = "+laserRef.transform.position.y);
        Destroy(warning);
        yield return new WaitForSeconds(1.5f);
        Destroy(laserRef);
    }
}
