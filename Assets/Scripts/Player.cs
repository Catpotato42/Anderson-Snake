using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Dependencies.Sqlite;

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
    private float temporaryTime = 0f;
    private float localTimeScale;
    private bool paused = true;
    private bool isChoosing = false;

    private bool canChangeDirection = true;

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
        Debug.Log("High score: "+PlayerPrefs.GetInt("highScore"));
        Debug.Log("Unlocked Everett: "+PlayerPrefs.GetInt("scoreEverett"));
        Debug.Log("Everett high score: "+PlayerPrefs.GetInt("eHighScore"));    
        if (difficultyScale == "everett") {
            difficultyTime = .015f;
        } else if (difficultyScale == "basic") {
            difficultyTime = .005f;
            Debug.Log("Difficulty = basic");
        } else {
            Debug.Log("Error: float difficultyTime not found.");
            difficultyTime = .001f;
        }
        Debug.Log("now resetting snake");
        ResetSnake();
        paused = false;
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
        isDead = false;
        upgradeNumber = 0;
        GameManager.instance.MapSize = PlayerPrefs.GetInt("mapSize") + 6;
        TileMapper.instance.RefreshTileMap();
        OnReset.Invoke();
        GameManager.instance.SetDictionaryValues(); //called in awake of gamemanager too, probably redundant.
        //position , rotation, direction, time
        transform.position = new Vector2(0, 0);
        transform.rotation = Quaternion.Euler(0,0,-90);
        direction = Vector2.right;
        Time.timeScale = .1f;
        deathCanvas.SetActive(false);
        SetScore(0);
        ResetSegments();
        lastInput = "D";
    }

    void ResetSegments () {
        //destroys segments
        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }
        segments.Clear(); //removes segments
        segments.Add(gameObject); //adds head (pause)

        //puts initial segments after head
        for (int i = 0; i < PlayerPrefs.GetInt("extraSegments"); i++) {
            //AddScore(1);
            Grow();
        }
        Time.timeScale = .1f;
    }

    public void Grow () {
        GameObject newSegment = Instantiate(segment);
        newSegment.transform.position = segments[segments.Count - 1].transform.position; //the position is exactly where the old one currently is.
        segments.Add(newSegment);
        if(Time.timeScale == temporaryTime) {
            Time.timeScale = localTimeScale;
            isChoosing = false;
        }
        if (difficultyScale == "everett" && Time.timeScale <= .2f) {
            Time.timeScale += difficultyTime;
        }
        else if (difficultyScale == "basic" && Time.timeScale <= .1f){
            Time.timeScale += difficultyTime;
        } else if (Time.timeScale <= .25f && difficultyScale == null || difficultyScale == "") {
            Time.timeScale += difficultyTime;
        }
    }

    void Update()
    {
        GetUserInput();
        if (canChangeDirection) {
            ProcessInputQueue();
        }
    }

    void FixedUpdate () {
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
        } else if (Input.GetKeyDown(KeyCode.Space)) { //doesn't work right now because Time.timeScale freezes the game so frames don't progress
            if (paused && !isChoosing && !isDead) {
                Time.timeScale = localTimeScale;
                paused = false;
            } else if (!paused && !isChoosing && !isDead) {
                localTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                paused = true;
            } else if (isChoosing || isDead) {
                
            }
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
        if (collide.CompareTag("Obstacle")) {
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
                localTimeScale = Time.timeScale;
                Time.timeScale = temporaryTime;
                OnUpgrade.Invoke(true);
            }
            //Debug.Log("Score = "+snakeScore);
        }
    }
}
