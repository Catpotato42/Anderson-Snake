using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.MPE;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{

    Vector2 direction;

    [SerializeField] private GameObject segment;
    List<GameObject> segments = new List<GameObject>();

    private GameObject highScoreObj;

    private Queue<KeyCode> inputQueue = new Queue<KeyCode>();

    public event Action<int> OnScoreChanged;
    public event Action<bool> OnEverettUnlock;
    public event Action<bool> OnUpgrade;
    public event Action OnReset;

    private string lastInput = "D";

    private int snakeScore = 0;
    private List<int> growThreshold = new List<int>{5, 10, 15, 20, 25, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 175, 190, 205, 220, 235};
    private int upgradeNumber = 0;

    [SerializeField] private GameObject deathCanvas;
    private bool isDead = false;
    private DeathScreen deathScreen;

    private float difficultyTime;
    private string difficultyScale;
    private float customFixedInterval = 0.02f; // Time interval in seconds (same as default Time.fixedDeltaTime so should run fixedupdate at the same rate)
    private float timeSinceLastUpdate = 0f;
    private float localTimeScale;
    private bool paused = true;
    private bool isChoosing = false;

    [SerializeField] private GameObject bulletPrefab;
    private float fireForce = 20f;
    private float fireCooldown = 2f;
    private float fireCooldownTracker = 0f;
    private bool canShoot = false;

    private bool dashing = false;
    private int dashAmount = 3;
    private int dashAmountTracker = 3;
    private bool canDash = false;
    private float dashCooldown = 1f;
    private float dashCooldownTracker = 0f;
    private float dashSpeed = .5f;
    private float tempDashTime;


    private bool canChangeDirection = true;

    public float FireCooldown {
        get => fireCooldown;
        set {
            fireCooldown = value;
        }
    }

    public float DifficultyTime {
        get => difficultyTime;
    }


    public float LocalTimeScale {
        get => localTimeScale;
        set {
            localTimeScale = value;
        }
    }

    public int UpgradeNumber {
        get => upgradeNumber;
        set {
            upgradeNumber = value;
        }

    }

    public int SnakeScore {
        get => snakeScore; 
        set {
            if (snakeScore != value){
                snakeScore = value;
                // Invoke the event whenever the score changes
                OnScoreChanged.Invoke(snakeScore);
            }
        }
    }

    void Awake () {
        highScoreObj = GameObject.FindGameObjectWithTag("HighScore");
        TileMapper.instance.RefreshTileMap(); //needs to be done in awake so that the food doesn't spawn wrong
        if (deathCanvas != null) {
            deathScreen = deathCanvas.GetComponent<DeathScreen>();
        }
        //change skin, segments has its own script for this.
        SpriteRenderer skin = gameObject.GetComponent<SpriteRenderer>();
        if (GameManager.instance.SkinPref == "everett") {
            skin.sprite = Resources.Load<Sprite>("Skins/EverettHead");
        } else {
            skin.sprite = Resources.Load<Sprite>("Skins/Square");
        }
        difficultyScale = GameManager.instance.SkinPref;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Extra segments: "+PlayerPrefs.GetInt("extraSegments"));
        //Debug.Log("High score: "+PlayerPrefs.GetInt("highScore"));
        Debug.Log("Unlocked Everett: "+PlayerPrefs.GetInt("scoreEverett"));
        //Debug.Log("Everett high score: "+PlayerPrefs.GetInt("eHighScore"));    
        if (difficultyScale == "everett") {
            difficultyTime = .015f;
            Debug.Log("Difficulty = everett");
        } else if (difficultyScale == "basic") {
            difficultyTime = .008f;
            Debug.Log("Difficulty = basic");
        } else {
            Debug.Log("Error: float difficultyTime not found.");
            difficultyTime = .001f;
        }
        Debug.Log("now resetting snake");
        ResetSnake();
    }

    private void SetScore (int newScore) {
        SnakeScore = newScore;
    }

    private void AddScore (int newScore) {
        SnakeScore += newScore;
    }

    private void SubtractScore (int newScore) {
        SnakeScore -= newScore;
    }

    public void ResetSnake () {
        //isDead = false;
        upgradeNumber = 0;
        GameManager.instance.MapSize = PlayerPrefs.GetInt("mapSize") + 6;
        TileMapper.instance.RefreshTileMap();
        GameManager.instance.SetDictionaryValues(); //called in awake of gamemanager too, probably redundant.
        //position , rotation, direction, time
        transform.position = new Vector2(0, 0);
        transform.rotation = Quaternion.Euler(0,0,-90);
        direction = Vector2.right;
        localTimeScale = .1f;
        Time.timeScale = 1f;
        deathCanvas.SetActive(false);
        SetScore(0);
        ResetSegments();
        lastInput = "D";
        OnReset.Invoke(); //needs to be invoked AFTER RefreshTileMap and pos, rot... as player would see old tilemap and other stuff on countdown otherwise
        paused = false;
        isDead = false;
    }

    void ResetSegments () {
        //destroys segments
        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }
        segments.Clear(); //removes segments
        segments.Add(gameObject); //adds head (pause)

        //puts initial segments after head
        for (int i = 0; i < PlayerPrefs.GetInt("extraSegments") + 1; i++) {
            Grow();
        }
        localTimeScale = .1f;
    }

    public void Grow () {
        if (segments.Count == 1) {
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = transform.position - (Vector3)direction;;
            segments.Add(newSegment);
        } else {
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = segments[segments.Count - 1].transform.position;
            segments.Add(newSegment);
        }
        if(Time.timeScale == 0) {
            Time.timeScale = 1f;
            isChoosing = false;
        }
        if (difficultyScale == "everett" && localTimeScale <= .3f) {
            localTimeScale += difficultyTime;
        }
        else if (difficultyScale == "basic" && localTimeScale <= .25f){
            localTimeScale += difficultyTime;
        } else if (localTimeScale <= .25f && difficultyScale == null || difficultyScale == "") {
            Debug.Log("problem in grow function of player script, difficulty scale = "+ difficultyScale);
            localTimeScale += difficultyTime;
        }
    }

    public void DoneChoosing () { //allows rainbowboa upgrades to unpause the game without growing the player
        if(Time.timeScale == 0) {
            Time.timeScale = 1f;
            isChoosing = false;
        }
    }

    public void RemoveSegment () {
        if (segments.Count <= 2 && segments.Count > 0) {
            return;
        } else if (segments.Count > 2) {
            GameObject segmentToRemove = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1); //I'm not sure but this might be able to be implemented with segments.Remove(segments.FindLastIndex(segment));
            Destroy(segmentToRemove); //This needs to be after removal from the list or else the RemoveAt is dereferencing a null pointer.
        } else {
            Debug.Log("Error: Tried to call RemoveSegment (Player.cs public void RemoveSegment()) but segment.Count < 1 (no head (huh??)). segments.Count = " + segments.Count);
        }
    }

    private void Fire () {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; //there has to be a way to just get Vector2 but this'll do
        Vector3 direction = mousePosition - transform.position;
        Quaternion rot = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
        bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * fireForce;
    }

    void Update() {
        fireCooldownTracker += Time.deltaTime; //if TimeScale is 0, this won't increment as opposed to unscaledDeltaTime which would.
        if (fireCooldownTracker > fireCooldown) {
            canShoot = true;
        }
        dashCooldownTracker += Time.deltaTime;
        if (dashCooldownTracker > dashCooldown) {
            canDash = true;
        }
        GetUserInput();
        if (canChangeDirection) {
            ProcessInputQueue();
        }
    }

    void FixedUpdate() {
        // Using Time.fixedDeltaTime to accumulate time
        timeSinceLastUpdate += Time.fixedDeltaTime;
        //if timesincelast > .02 (normal amount for fixed update) / localtime (.1 or something, so it takes 10x longer to perform an action.)
        if (timeSinceLastUpdate >= customFixedInterval/localTimeScale) {
            PerformCustomFixedUpdateLogic();

            timeSinceLastUpdate = 0f;
        }
    }

    void PerformCustomFixedUpdateLogic() {
        if (dashing) {
            dashAmountTracker--;
        } if (dashAmountTracker <= 0) {
            dashing = false;
            dashAmountTracker = dashAmount;
            localTimeScale = tempDashTime;
            tempDashTime = 0; //unnecessary
        }
        MoveSegments();
        MoveSnake();
    }

    void GetUserInput () {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            QueueInput(KeyCode.W);
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            QueueInput(KeyCode.S);
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            QueueInput(KeyCode.D);
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            QueueInput(KeyCode.A);
        } else if (Input.GetKeyDown(KeyCode.R) && isDead || Input.GetKeyDown(KeyCode.Space) && isDead) {
            ResetSnake();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isDead) {
            SceneManager.LoadScene(0);
            PlayerPrefs.SetInt("mapSize", 0);
        } else if (Input.GetKeyDown(KeyCode.Space) && canDash) { //pausing is only in for testing purposes! Dashing is the intended functionalty of space
            /*if (paused && !isChoosing && !isDead) {
                Time.timeScale = 1f;
                paused = false;
            } else if (!paused && !isChoosing && !isDead) {
                Time.timeScale = 0f;
                paused = true;
            } else if (isChoosing || isDead) {
                
            }*/
            dashCooldownTracker = 0f;
            dashing = true;
            canDash = false;
            tempDashTime = localTimeScale;
            localTimeScale = dashSpeed;
        } else if (Input.GetKeyDown(KeyCode.Mouse0) && !isChoosing && !isDead && canShoot) {
            Fire();
            fireCooldownTracker = 0f; //needs to be before canShoot or maybe canShoot would set itself to true again?
            canShoot = false;
        }
    }

    void QueueInput(KeyCode key)
    {
        inputQueue.Enqueue(key);
    }

    void ProcessInputQueue()
    {
        while (inputQueue.Count > 0)
        {
            KeyCode input = inputQueue.Dequeue();
            HandleInput(input);
            
        }
    }

    void HandleInput(KeyCode input) {

        switch (input)
        {
            case KeyCode.W:
                if (lastInput == "S") break;
                lastInput = "W";
                direction = Vector2.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case KeyCode.A:
                if (lastInput == "D") break;
                lastInput = "A";
                direction = Vector2.left;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case KeyCode.S:
                if (lastInput == "W") break;
                lastInput = "S";
                direction = Vector2.down;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case KeyCode.D:
                if (lastInput == "A") break;
                lastInput = "D";
                direction = Vector2.right;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
        canChangeDirection = false;
    }
    void MoveSnake () {
        float x = transform.position.x + direction.x;
        float y = transform.position.y + direction.y;
        transform.position = new Vector2 (x, y);
        canChangeDirection = true;
    }

    void MoveSegments () {
        //put on top of one in front
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].transform.position = segments[i - 1].transform.position;
        }
    }

    void updateHighScore () {
        string tempHighScore;
        if (difficultyScale == "everett") {
            tempHighScore = "eHighScore";
        } else if (difficultyScale == "basic") {
            tempHighScore = "HighScore";
        } else {
            tempHighScore = null;
        }
        if (PlayerPrefs.GetInt(tempHighScore) < snakeScore) {
            //Debug.Log(snakeScore+" snakeScore higher than "+PlayerPrefs.GetInt("highScore"));
            PlayerPrefs.SetInt(tempHighScore, snakeScore);
            HighScore highScore = highScoreObj.GetComponent<HighScore>();
            highScore.updateHighScore(tempHighScore);
        }
    }

    void OnTriggerEnter2D (Collider2D collide) {
        if (collide.CompareTag("Obstacle") || collide.CompareTag("Walls")) {
            Time.timeScale = 0f;
            isDead = true;
            Debug.Log(upgradeNumber);
            deathScreen.Setup(snakeScore);
            updateHighScore();
        } else if (collide.CompareTag("Food")) {
            AddScore(1);
            updateHighScore();
            if (snakeScore == 10 && PlayerPrefs.GetInt("scoreEverett") != 1) {
                PlayerPrefs.SetInt("scoreEverett", 1);
                OnEverettUnlock.Invoke(true);
            }
            if (snakeScore >= growThreshold[upgradeNumber]) {
                Debug.Log("Hit threshold "+upgradeNumber+" at score "+snakeScore);
                upgradeNumber++;
                isChoosing = true;
                Time.timeScale = 0f;
                OnUpgrade.Invoke(true);
            }
            //Debug.Log("Score = "+snakeScore);
        }
    }
}
