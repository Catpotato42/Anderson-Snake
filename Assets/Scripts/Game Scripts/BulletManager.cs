using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletManager : MonoBehaviour
{
    //yoinked this from the LaserManager script that I spent like 4 hours on
    private float timeTracker = 0;
    private int bulletWaveTracker = 0;
    [SerializeField] private GameObject bulletWarning;
    [SerializeField] private GameObject bullet;
    private BulletWaves bulletWave;
    private List<int> lt;
    private float duration;

    private class BulletWaves {
        public int Amount;
        public float WarningAmount;
        public float SpawnTime;
        public BulletWaves (int amount, float warningAmount, float spawnTime) {
            Amount = amount;
            WarningAmount = warningAmount;
            SpawnTime = spawnTime;
        }
    }

    void Awake() {
        InitializeBulletWaves();
    }

    void Start () {
        Player.instance.OnReset += InitializeBulletWaves;
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

    private void InitializeBulletWaves () {
        StopAllCoroutines();
        timeTracker = 0;
        bulletWaveTracker = 0;
        bulletWave = new BulletWaves(1, 10, 3);
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("BasicBullet")) {
            Destroy(temp);
        }
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Warning")) {
            Destroy(temp);
        }
    }

    void Update () {
        timeTracker += Time.deltaTime;
        if (timeTracker >= bulletWave.WarningAmount) {
            //Debug.Log(" bullet timeTracker = "+timeTracker+", going to spawn new wave "+bulletWaveTracker);
            //reset tracker
            timeTracker = 0;
            bulletWaveTracker++;
            //Debug.Log("Bullet past tracker sets");
            //do wave logic
            WaveSpawning(bulletWave);
            //Debug.Log(bulletWave.Amount+" bullets should've been spawned.");
            //set next wave
            //these values should be set depending on how many waves have passed
            NextBulletWave();
        }
    }

    private void NextBulletWave() {
        int nextAmount = 1;
        float nextWarningAmount = 2;
        float nextSpawnTime = 3;
        if (bulletWaveTracker < lt[0]) {
            nextAmount = 1;
            nextWarningAmount = Random.Range(10, 20);
            nextSpawnTime = duration;
        } else if (bulletWaveTracker >= lt[0] && bulletWaveTracker < lt[1]) {
            nextAmount = 2;
            nextWarningAmount = Random.Range(10, 20);
            nextSpawnTime = duration;
        } else if (bulletWaveTracker >= lt[1] && bulletWaveTracker < lt[2]) {
            if (GameManager.instance.MapSizeTemp - 3 > 4) {
                nextAmount = Random.Range(4,GameManager.instance.MapSizeTemp - 3); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 4;
            }
            nextWarningAmount = Random.Range(8, 18);
            nextSpawnTime = duration;
        } else if (bulletWaveTracker >= lt[2] && bulletWaveTracker < lt[3]) {
            if (GameManager.instance.MapSizeTemp - 2 > 6) {
                nextAmount = Random.Range(6,GameManager.instance.MapSizeTemp - 2); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 3;
            }
            nextWarningAmount = Random.Range(8, 18);
            nextSpawnTime = duration;
        } else if (bulletWaveTracker >= lt[3]){
            if (GameManager.instance.MapSizeTemp - 2 > 10) {
                nextAmount = Random.Range(10,GameManager.instance.MapSizeTemp - 2); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 3;
            }
            nextWarningAmount = Random.Range(6, 16);
            nextSpawnTime = duration;
        }
        if (bulletWaveTracker >= lt[3] + 10 && (GameManager.instance.Difficulty == "hard" || GameManager.instance.Difficulty == "everett")) {
            if (GameManager.instance.MapSizeTemp > 12) {
                nextAmount = Random.Range(12,GameManager.instance.MapSizeTemp - 1); //max exclusive
            } else {
                nextAmount = GameManager.instance.MapSizeTemp - 2;
            }
            nextWarningAmount = Random.Range(4, 16);
            nextSpawnTime = 1f;
        }
        //then set bulletWave to a new Tuple containing these.
        bulletWave = new BulletWaves(nextAmount, nextWarningAmount, nextSpawnTime);
    }

    private void WaveSpawning (BulletWaves currWave) {
        //Debug.Log("'Hacker voice': I'm in IEnumerator WaveSpawning");
        for (int i = 0; i < currWave.Amount; i++) {
            StartCoroutine(LocationValues(currWave));
        }
    }

    private System.Collections.IEnumerator LocationValues (BulletWaves currWave) {
        //waits a random interval between warning bullet spawns to make it look visually interesting, added onto amount of time to wait to spawn the real bullet in coroutine
        float randomTime = Random.Range(0, 101)/100f; //max exclusive
        //Debug.Log("boutta wait for "+randomTime+" seconds");
        yield return new WaitForSeconds(randomTime);
        //Debug.Log("this stupid ass code won't ");
        Vector3 location = CalculateLocation(currWave);
        //Debug.Log("location: "+location);
        GameObject warningObject = Instantiate(bulletWarning, location, Quaternion.identity);
        StartCoroutine(SpawnBullet(currWave, randomTime, location, warningObject));
    }

    private System.Collections.IEnumerator SpawnBullet (BulletWaves currWave, float extraTime, Vector3 location, GameObject warning) {
        //Debug.Log("in IEnum SpawnBullet, SpawnTime = "+currWave.SpawnTime+", extraTime (randomTime) = "+extraTime);
        yield return new WaitForSeconds(currWave.SpawnTime - extraTime);
        //in case the center has changed due to the map size changing by 1.
        GameObject bulletRef  = Instantiate(bullet, location, Quaternion.identity);
        Rigidbody2D rbBullet = bulletRef.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(new Vector2(5, 4.98f), ForceMode2D.Impulse);
        //Debug.Log("bullet location x= "+bulletRef.transform.position.x+", bullet location y = "+bulletRef.transform.position.y);
        Destroy(warning);
        yield return new WaitForSeconds(60);
        Destroy(bulletRef); //won't throw an error
    }

    private Vector3 CalculateLocation (BulletWaves currWave) {
        Vector3 location;
        int x = Random.Range(-(GameManager.instance.MapSizeTemp / 2) + 1, GameManager.instance.MapSizeTemp / 2);
        int y = Random.Range(-(GameManager.instance.MapSizeTemp / 2) + 1, GameManager.instance.MapSizeTemp / 2);
        location = new Vector3(x, y, 0);
        return location;
    }
}
