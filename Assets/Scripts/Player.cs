using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour, ISaveManager
{

    //TODO:
    //implement a red screen if you hit something and you have health left.
    //upgrades for timer that make it longer so you balance utility with a longer time. 
    //BUGS:
    //If you input a direction you are already going, then another one right after that, it takes one segment move for it to update.
    //DDSA, see Input Handling.

    public static Player instance;
    Vector2 direction;

    [SerializeField] private GameObject segment;
    List<GameObject> segments = new List<GameObject>();

    private GameObject highScoreObj;
    private HighScore highScoreScript;

    private Queue<KeyCode> inputQueue = new Queue<KeyCode>();

    public event Action<int> OnScoreChanged;
    public event Action<string> OnDiffUnlock;
    public event Action<bool> OnUpgrade;
    public event Action OnReset;

    private string lastInput = "D";

    private int snakeScore = 0;
    private List<int> growThreshold = new List<int>();
    private int upgradeNumber = 0;

    [SerializeField] private GameObject deathCanvas;
    private bool isDead = false;
    private DeathScreen deathScreen;

    private float difficultyTime;
    private string difficultyScale;
    private float customFixedInterval = 0.02f; // Time interval in seconds (same as default Time.fixedDeltaTime so should run fixedupdate at the same rate)
    private float timeSinceLastUpdate = 0f;
    private float localTimeScale;
    //private bool paused = true;
    private bool isChoosing = false;

    [SerializeField] private GameObject bulletPrefab;
    private bool shootingEnabled = false;
    private float fireForce = 10f;
    private float fireCooldown = 2f;
    private float fireCooldownTracker = 0f;
    private bool canShoot = false;

    private bool hasReverse;

    private bool hasDash;
    private bool hasDashInvincibility;

    private bool dashing = false;
    private int dashAmount = 3;
    private int dashAmountTracker = 3;
    private bool canDash = false;
    private float dashCooldown = 2f;
    public float DashCooldown {
        get =>dashCooldown; 
        set{dashCooldown = value;}
    }
    private float dashCooldownTracker = 2f;
    private float dashSpeed = .5f;
    private float tempDashTime;

    private bool canReverse;
    private float reverseCooldown = .3f;
    private float reverseCooldownTracker = 0;

    private float health = 50f; //set on reset
    public float Health {get => health; set => health = value;}
    private static float invincTimer = .2f;
    private float invincTracker = invincTimer;

    private float extraHealth = 0;
    private int extraSegments;

    private Vector2 lastSegmentDirection;

    private bool hasEverett;
    private bool hasMedium;
    private bool hasHard;
    private int highScore;
    public int HighScore {
        get => highScore;
    }

    private bool canChangeDirection = true;

    public float FireCooldown {
        get => fireCooldown;
        set {fireCooldown = value;}
    }

    public float DifficultyTime {
        get => difficultyTime;
    }


    public float LocalTimeScale {
        get => localTimeScale;
        set {localTimeScale = value;}
    }

    public int UpgradeNumber {
        get => upgradeNumber;
        set {upgradeNumber = value;}

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
    private void SetScore (int newScore) {
        SnakeScore = newScore;
    }

    private void AddScore (int newScore) {
        SnakeScore += newScore;
    }

    private void SubtractScore (int newScore) {
        SnakeScore -= newScore;
    }

    public void LoadData (GameData data) {
        extraHealth = data.extraHealth;
        hasMedium = data.hasMedium;
        hasHard = data.hasHard;
        hasEverett = data.hasEverett;
        extraSegments = data.extraSegments;
        hasDash = data.hasDash;
        hasDashInvincibility = data.hasDashInvincibility;
        hasReverse = data.hasReverse;
    }
    public void SaveData(GameData data) {
        data.hasMedium = hasMedium;
        data.hasHard = hasHard;
        data.hasEverett = hasEverett;
    }

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        //game should load and set health, amount of segments, ...
        highScoreObj = GameObject.FindGameObjectWithTag("HighScore");
        highScoreScript = highScoreObj.GetComponent<HighScore>();
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
        difficultyScale = GameManager.instance.Difficulty;
        InitThresholdValues();
    }

    private void InitThresholdValues () {
        growThreshold.Add(5);
        for (int i = 1; i < 5; i++) {
            growThreshold.Add(growThreshold[i - 1] + 5);
        } for (int i = 5; i < 10; i++) {
            growThreshold.Add(growThreshold[i - 1] + 10);
        } for (int i = 10; i < 15; i++) {
            growThreshold.Add(growThreshold[i - 1] + 20);
        } for (int i = 15; i < 30; i++) {
            growThreshold.Add(growThreshold[i - 1] + 30);
        }
        string printThresholdVals = "";
        for (int i = 0; i < growThreshold.Count; i++) {
            printThresholdVals += growThreshold[i]+", ";
        }
        Debug.Log("Threshold vals = "+printThresholdVals);
    }
    
    // Start is called before the first frame update
    void Start()
    {   
        if (difficultyScale == "everett") {
            difficultyTime = .015f;
            Debug.Log("Difficulty = everett");
        } else if (difficultyScale == "basic") {
            difficultyTime = .008f;
            Debug.Log("Difficulty = basic");
        } else if (difficultyScale == "medium") {
            difficultyTime = .01f;
            Debug.Log("Difficulty = medium");
        } else if (difficultyScale == "hard") {
            difficultyTime = .012f;
            Debug.Log("Difficulty = hard");
        } else {
            Debug.Log("Error: float difficultyTime not found.");
            difficultyTime = .001f;
        }
        //Debug.Log("now resetting snake");
        ResetSnake();
    }

    public void ResetSnake () {
        //isDead = false;
        canDash = false;
        canShoot = false;
        canReverse = false;
        upgradeNumber = 0;
        dashCooldownTracker = 2f;
        invincTracker = invincTimer;
        health = extraHealth + 50f;
        GameManager.instance.MapSizeTemp = GameManager.instance.MapSize + 6;
        TileMapper.instance.RefreshTileMap();
        GameManager.instance.SetDictionaryValues(); //called in awake of gamemanager too, probably redundant.
        UpdateHighScore(); //this is called in Countdown too
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
        //paused = false;
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
        for (int i = 0; i < extraSegments + 10; i++) { //extraSegments + 2 for 1 segment
            Grow();
        }
        localTimeScale = .1f;
    }

    public void Grow () {
        if (segments.Count == 1) {
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = transform.position - (Vector3)direction;
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
        if (difficultyScale == "everett" && localTimeScale <= .4f) {
            localTimeScale += difficultyTime;
        } else if (difficultyScale == "basic" && localTimeScale <= .25f){
            localTimeScale += difficultyTime;
        } else if (difficultyScale == "medium" && localTimeScale <= .28f){
            localTimeScale += difficultyTime;
        } else if (difficultyScale == "hard" && localTimeScale <= .33f){
            localTimeScale += difficultyTime;
        } else if (localTimeScale <= .25f && difficultyScale == null || difficultyScale == "") {
            Debug.Log("problem in grow function of player script, difficulty scale = "+ difficultyScale);
            localTimeScale += difficultyTime;
        }
    }

    public void Grow (int segmentsToGrow) {
        if (segments.Count == 1) { //first segment, segmentsToGrow-- so growing the first segment counts as one
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = transform.position - (Vector3)direction;
            segments.Add(newSegment);
            segmentsToGrow--;
        }
        for (int i = 0; i < segmentsToGrow; i++) { //
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = segments[segments.Count - 1].transform.position;
            segments.Add(newSegment);
        }
        if(Time.timeScale == 0) {
            Time.timeScale = 1f;
            isChoosing = false;
        }
        if (difficultyScale == "everett" && localTimeScale <= .4f) {
            localTimeScale += difficultyTime;
        } else if (difficultyScale == "basic" && localTimeScale <= .25f){
            localTimeScale += difficultyTime;
        } else if (difficultyScale == "medium" && localTimeScale <= .28f){
            localTimeScale += difficultyTime;
        } else if (difficultyScale == "hard" && localTimeScale <= .33f){
            localTimeScale += difficultyTime;
        } else if (localTimeScale <= .25f && difficultyScale == null || difficultyScale == "") {
            Debug.Log("problem in grow function of player script, difficulty scale = "+ difficultyScale);
            localTimeScale += difficultyTime;
        }
    }

    private void GrowNoTime () {
        if (segments.Count == 1) {
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = transform.position - (Vector3)direction;
            segments.Add(newSegment);
        } else {
            GameObject newSegment = Instantiate(segment);
            newSegment.transform.position = segments[segments.Count - 1].transform.position;
            segments.Add(newSegment);
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
            segments.RemoveAt(segments.Count - 1); //I'm not sure but this might be able to be implemented with segments.Remove(segments.FindLastIndex(segment)); no point in doing that but just I should prolly know
            Destroy(segmentToRemove); //This needs to be after removal from the list or else the RemoveAt index is null.
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
        if (fireCooldownTracker > fireCooldown && !canShoot) {
            canShoot = true;
        }
        dashCooldownTracker += Time.deltaTime;
        if (dashCooldownTracker > dashCooldown && !canDash) {
            canDash = true;
        }
        reverseCooldownTracker += Time.deltaTime;
        if (reverseCooldownTracker > reverseCooldown && !canReverse) {
            canReverse = true;
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
            DoFixedUpdateStuff();
            timeSinceLastUpdate = 0f;
        }
    }

    void DoFixedUpdateStuff() {
        DecreaseDashAmount();
        MoveSegments();
        MoveSnake();
    }

    private void DecreaseDashAmount () {
        if (dashing) {
            dashAmountTracker--;
        } if (dashAmountTracker <= 0) {
            dashing = false;
            dashAmountTracker = dashAmount;
            localTimeScale = tempDashTime;
            tempDashTime = 0; //unnecessary
        }

    }

    private void GetUserInput () {
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
            SaveManager.instance.SaveGame();
            SceneManager.LoadScene(0);
        } else if (Input.GetKeyDown(KeyCode.Space) && canDash && hasDash) { //pausing is only in for testing purposes! Dashing is the intended functionalty of space
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
        } else if (Input.GetKeyDown(KeyCode.Mouse0) && !isChoosing && canShoot && shootingEnabled) {
            Fire();
            fireCooldownTracker = 0f; //needs to be before canShoot or maybe canShoot would set itself to true again?
            canShoot = false;
        } else if (Input.GetKeyDown(KeyCode.F) && canReverse && hasReverse) {
            inputQueue.Clear();
            QueueInput(KeyCode.F);
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
        //DDSA BUG: You can trick the game into for example thinking you pressed S and 
        //moved one square down already if you hit ddsa really fast.
        //How to fix: maybe track actual movement? then I would be able to stop this,
        //but it would require a complete overhaul of the input system. 
        //Definitely something reserved for like actual post-Demo or even full release bugfixing.
        //It's a big edge case and it will affect you at the start of the game if you hit dsa really fast because the first input is already
        //set to "D", but it's also very easy to avoid and I don't think would really make anyone scared of inputting too fast.
        //Currently LOW-priority.
        switch (input) {
            case KeyCode.W:
                if (lastInput == "S") {
                    break;
                }
                lastInput = "W";
                direction = Vector2.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case KeyCode.A:
                if (lastInput == "D") {
                    break;
                }
                lastInput = "A";
                direction = Vector2.left;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case KeyCode.S:
                if (lastInput == "W") {
                    break;
                }
                lastInput = "S";
                direction = Vector2.down;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case KeyCode.D:
                if (lastInput == "A") {
                    break;
                }
                lastInput = "D";
                direction = Vector2.right;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case KeyCode.F:
                StartCoroutine(ReverseSnake());
                reverseCooldownTracker = 0;
                canReverse = false;
                break;
        }
        canChangeDirection = false;
    }
    private void MoveSnake () {
        float x = transform.position.x + direction.x;
        float y = transform.position.y + direction.y;
        transform.position = new Vector2 (x, y);
        canChangeDirection = true;
    }

    private void MoveSegments () {
        //put on top of one in front
        for (int i = segments.Count - 2; i > 0; i--) {
            segments[i].transform.position = segments[i - 1].transform.position;
        }
        Vector3 oldPosLast = segments[segments.Count - 1].transform.position; //getting the direction 
        segments[segments.Count - 1].transform.position = segments[segments.Count - 2].transform.position;
        Vector3 newPosLast = segments[segments.Count - 1].transform.position;
        float diffPosX = newPosLast.x - oldPosLast.x;
        float diffPosY = newPosLast.y - oldPosLast.y;
        if (diffPosX != 0) {
            diffPosX = diffPosX/MathF.Abs(diffPosX);
        }
        if (diffPosY != 0) {
            diffPosY = diffPosY/MathF.Abs(diffPosY);
        }
        lastSegmentDirection = new Vector2(diffPosX, diffPosY);
    }

    private IEnumerator ConfuseSnake() { //used to be ReverseSnake but better would be reversing from head
        //switch head and tail
        yield return new WaitForEndOfFrame(); //so that all segments are moved
        if (Time.timeScale > 0f) {
            float tempTime = Time.timeScale;
            Time.timeScale = 0f;
            segments[0].transform.position = segments[segments.Count - 1].transform.position;
            segments.Reverse(1, segments.Count - 2);
            if (lastSegmentDirection == Vector2.up) {
                direction = Vector2.down;
            } else if (lastSegmentDirection == Vector2.right) {
                direction = Vector2.left;
            } else if (lastSegmentDirection == Vector2.down) {
                direction = Vector2.up;
            } else if (lastSegmentDirection == Vector2.left) {
                direction = Vector2.right;
            }
            MoveSnake();
            Time.timeScale = tempTime;
        }
        canChangeDirection = true;
    }

    private IEnumerator ReverseSnake() { //changes direction after storing how many segments there are and regrows the snake from the current head position. 
        //it might be technically both more processing intensive (VERY negligable) and look a little weird, but it allows immediate reverses one after another
        //AND it fits better
        
        //switch head and tail
        yield return new WaitForEndOfFrame(); //so that all segments are moved
        if (Time.timeScale > 0f) {
            float tempTime = Time.timeScale;
            Time.timeScale = 0f;
            //segments[0].transform.position = segments[segments.Count - 1].transform.position;
            //segments.Reverse(1, segments.Count - 2);
            int currSegCount = segments.Count - 1;
            //destroys segments
            for (int i = 1; i < segments.Count; i++) {
                Destroy(segments[i].gameObject);
            }   
            segments.Clear(); //removes segments
            segments.Add(gameObject); //adds head (pause)
            if (direction == Vector2.up) {
                direction = Vector2.down;
                lastInput = "S";
            } else if (direction == Vector2.right) {
                direction = Vector2.left;
                lastInput = "A";
            } else if (direction == Vector2.down) {
                direction = Vector2.up;
                lastInput = "W";
            } else if (direction == Vector2.left) {
                direction = Vector2.right;
                lastInput = "D";
            }
            MoveSnake();
            for (int i = 0; i < currSegCount; i++) {
               GrowNoTime();
            }
            Time.timeScale = tempTime;
        }
        canChangeDirection = true;
    }

    void UpdateHighScore () { //High score script handles the individual high scores and separates them for us.
        highScore = highScoreScript.GetHighScore(difficultyScale);
        if (highScore < snakeScore) {
            //Debug.Log(snakeScore+" snakeScore higher than "+PlayerPrefs.GetInt("highScore"));
            highScore = snakeScore;
            highScoreScript.UpdateHighScore(difficultyScale, highScore);
        } else {
            highScoreScript.UpdateHighScore(difficultyScale);
        }
    }

    public void OnPlayerHitLogic (string tag) {
        if ((tag == "Laser" || tag == "Obstacle") && hasDashInvincibility && dashing) {
            return;
        } else if (tag == "Walls" && hasDashInvincibility && dashing) {
            StartCoroutine(ConfuseSnake());
            canChangeDirection = false;
            return;
        }
        health -= 50;
        Debug.Log("health = "+health);
        if (health <= 0) {
            KillPlayer();
        } else {
            if (!(tag == "Laser")) {
                StartCoroutine(ReverseSnake());
                canChangeDirection = false;
            }
        }
    }

    public void KillPlayer () { //yeah
        Time.timeScale = 0f;
        isDead = true;
        //Debug.Log("died with "+upgradeNumber+" upgrades");
        deathScreen.Setup(snakeScore);
        UpdateHighScore();
    }

    void OnTriggerStay2D (Collider2D collide) { //for laser, increments and checks if the player is no longer invincible and if so calls OnPlayerHitLogic
        if (!collide.CompareTag("Laser")) { //if the tag isn't laser leave the function NOW
            return;
        } else {
            invincTracker += Time.deltaTime;
            if (invincTracker >= invincTimer) {
                invincTracker = 0;
                OnPlayerHitLogic(collide.tag); //this should always be "Laser".
            }
        }

    }

    void OnTriggerExit2D (Collider2D collide) {
        invincTracker = invincTimer;
    }

    void OnTriggerEnter2D (Collider2D collide) {
        if (collide.CompareTag("Obstacle") || collide.CompareTag("Walls")) {
            OnPlayerHitLogic(collide.tag);
        } else if (collide.CompareTag("Laser")) {
            if (invincTracker >= invincTimer) {
                invincTracker = 0;
                OnPlayerHitLogic(collide.tag); //this should always be "Laser".
            }
        } else if (collide.CompareTag("Food")) {
            AteFood();
            //does multithreading mean that 2 of these methods can run at the same time? TODO look what multithreading really is up
        }
    }

    private void AteFood () { //meant to just be called in OnTrigger, logic for post eating food
        AddScore(1);
        UpdateHighScore();
        if (snakeScore == 50 && !hasMedium) {
            OnDiffUnlock.Invoke("MEDIUM");
            hasMedium = true;
        } else if (snakeScore == 100 && !hasHard) {
            OnDiffUnlock.Invoke("HARD");
            hasHard = true;
        } else if (snakeScore == 200 && !hasEverett) {
            OnDiffUnlock.Invoke("EVERETT");
            hasEverett = true;
        }
        UpgradePlayer();
    }

    private void UpgradePlayer () { //logic for checking if we should upgrade then upgrading player. Meant to just be called in AteFood.
        if (snakeScore == growThreshold[upgradeNumber]) {
            if (upgradeNumber == growThreshold.Count - 1) {
                growThreshold.Add(growThreshold[upgradeNumber] + 50);
                Debug.Log("Added " + growThreshold[growThreshold.Count - 1] + " to growThreshold at "+ (growThreshold.Count - 1));
            }
            //Debug.Log("Hit threshold "+upgradeNumber+" at score "+snakeScore+", threshold was "+growThreshold[upgradeNumber]+" and next should be "+growThreshold[upgradeNumber+1]);
            upgradeNumber++; //1 after first upgrade, growThreshold.Count after last.
            isChoosing = true;
            Time.timeScale = 0f;
            OnUpgrade.Invoke(true);
        }
    }
}