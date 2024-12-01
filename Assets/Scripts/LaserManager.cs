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
    private float duration;

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
            duration = 4f;
            lt = new List<int> {5, 12, 18, 30};
        } else if (difficulty == "medium") {
            duration = 3f;
            lt = new List<int> {5, 9, 13, 20};
        } else if (difficulty == "hard") {
            duration = 2f;
            lt = new List<int> {3, 8, 10, 15};
        } else if (difficulty == "everett") {
            duration = 1.5f;
            lt = new List<int> {1, 3, 5, 10};
        }
    }

    private void InitializeLaserWaves () {
        StopAllCoroutines();
        timeTracker = 0;
        laserWaveTracker = 0;
        laserWave = new LaserWaves(1, 10, 3, true);
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Laser")) {
            Destroy(temp);
        }
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Warning")) {
            Destroy(temp);
        }
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
            if (GameManager.instance.MapSizeTemp - 5 > 1) { 
                //ok this takes a little explaining. at map size 6 this means there will always be one laser,
                //and even at map size 7 the same. so if you can stay at lower map sizes you get less lasers but have less space.
                nextAmount = Random.Range(1,GameManager.instance.MapSizeTemp - 5); //max exclusive
            } else { //this first else is true for temp map size > 5, so always true.
                nextAmount = 1;
            }
            nextWaveTime = Random.Range(10, 20);
            nextSpawnTime = duration;
            vertical = false;
        } else if (laserWaveTracker >= lt[0] && laserWaveTracker < lt[1]) {
            if (GameManager.instance.MapSizeTemp - 4 > 2) {
                nextAmount = Random.Range(2,GameManager.instance.MapSizeTemp - 4); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 5;
            }
            nextWaveTime = Random.Range(10, 20);
            nextSpawnTime = duration;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        } else if (laserWaveTracker >= lt[1] && laserWaveTracker < lt[2]) {
            if (GameManager.instance.MapSizeTemp - 3 > 4) {
                nextAmount = Random.Range(4,GameManager.instance.MapSizeTemp - 3); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 4;
            }
            nextWaveTime = Random.Range(8, 18);
            nextSpawnTime = duration;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        } else if (laserWaveTracker >= lt[2] && laserWaveTracker < lt[3]) {
            if (GameManager.instance.MapSizeTemp - 2 > 6) {
                nextAmount = Random.Range(6,GameManager.instance.MapSizeTemp - 2); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 3;
            }
            nextWaveTime = Random.Range(8, 18);
            nextSpawnTime = duration;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        } else if (laserWaveTracker >= lt[3]){
            if (GameManager.instance.MapSizeTemp - 2 > 10) {
                nextAmount = Random.Range(10,GameManager.instance.MapSizeTemp - 2); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 3;
            }
            nextWaveTime = Random.Range(6, 16);
            nextSpawnTime = duration;
            vertical = System.Convert.ToBoolean(Random.Range(0,2));
        }
        if (laserWaveTracker >= lt[3] + 10 && (GameManager.instance.Difficulty == "hard" || GameManager.instance.Difficulty == "everett")) {
            if (GameManager.instance.MapSizeTemp > 12) {
                nextAmount = Random.Range(12,GameManager.instance.MapSizeTemp - 1); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 2;
            }
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
        float randomTime = Random.Range(0, 101)/100f; //max exclusive
        //Debug.Log("boutta wait for "+randomTime+" seconds");
        yield return new WaitForSeconds(randomTime);
        //Debug.Log("this stupid ass code won't ");
        Vector3 location = CalculateLocation(currWave);
        //Debug.Log("location: "+location);
        GameObject warningObject = Instantiate(laserWarning, location, Quaternion.identity);
        if (currWave.Vertical) {
            warningObject.transform.localScale = new Vector2(1, GameManager.instance.MapSizeTemp);
        } else {
            warningObject.transform.localScale = new Vector2(GameManager.instance.MapSizeTemp, 1);
        }
        StartCoroutine(SpawnLaser(currWave, randomTime, location, warningObject));
    }

    private System.Collections.IEnumerator SpawnLaser (LaserWaves currWave, float extraTime, Vector3 location, GameObject warning) {
        //Debug.Log("in IEnum SpawnLaser, SpawnTime = "+currWave.SpawnTime+", extraTime (randomTime) = "+extraTime);
        yield return new WaitForSeconds(currWave.SpawnTime - extraTime);
        //in case the center has changed due to the map size changing by 1.
        if (currWave.Vertical) {
            location.y = CalculateOdd() - 1;
        } else {
            location.x = CalculateOdd();
        }
        GameObject laserRef  = Instantiate(laser, location, Quaternion.identity);
        if (currWave.Vertical) {
            laserRef.transform.localScale = new Vector2(1, GameManager.instance.MapSizeTemp);
        } else {
            laserRef.transform.localScale = new Vector2(GameManager.instance.MapSizeTemp, 1);
        }
        //Debug.Log("laser location x= "+laserRef.transform.position.x+", laser location y = "+laserRef.transform.position.y);
        Destroy(warning);
        yield return new WaitForSeconds(1.5f);
        Destroy(laserRef);
    }

    private Vector3 CalculateLocation (LaserWaves currWave) {
        float odd = CalculateOdd(); //calculate if the map size is odd, if it is returns 1 else .5, factors that into the center of location.
        Vector3 location;
        if (currWave.Vertical) {
            location = new Vector3(Random.Range(-(GameManager.instance.MapSizeTemp / 2) + 1, GameManager.instance.MapSizeTemp / 2), odd - 1, 0);
        } else {
            location = new Vector3(odd, Random.Range(-(GameManager.instance.MapSizeTemp / 2) + 1, GameManager.instance.MapSizeTemp / 2), 0);
        }
        return location;
    }

    private float CalculateOdd () { //kinda a scuffed way to do this but i forsee no problems with this so ¯\_(ツ)_/¯
        float calcOdd;
        if ((GameManager.instance.MapSizeTemp % 2) == 0) {
            calcOdd = .5f;
        } else {
            calcOdd = 1;
        }
        return calcOdd;
    }
}
