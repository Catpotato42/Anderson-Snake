using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Vector2 direction;

    [SerializeField] private GameObject segment;
    List<GameObject> segments = new List<GameObject>();

    private GameObject highScoreObj;

    private Queue<KeyCode> inputQueue = new Queue<KeyCode>();

    public event Action<int> OnScoreChanged;
    public event Action<bool> OnEverettUnlock;

    private string lastInput = "D";

    private int snakeScore = 0;

    private float difficultyTime;
    private string difficultyScale;
    //private float localTimeScale;

    private bool canChangeDirection = true;
    //private float directionChangeCooldown = 0.1f;
    //private float timeSinceLastChange = 0f;
    //private Vector2 lastDirection;

    public int SnakeScore {
        get => snakeScore; 
        set {
            if (snakeScore != value){
                snakeScore = value;
                // Invoke the event whenever the score changes
                OnScoreChanged?.Invoke(snakeScore);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        highScoreObj = GameObject.FindGameObjectWithTag("HighScore");

        difficultyScale = GameManager.instance.SkinPref;
        if (difficultyScale == "everett") {
            difficultyTime = .015f;
        } else if (difficultyScale == "basic") {
            difficultyTime = .005f;
        } else {
            Debug.Log("Error: float difficultyTime not found.");
            difficultyTime = .001f;
        }
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

    void ResetSnake () {
        //position , rotation, direction, time
        transform.position = new Vector2(0, 0);
        transform.rotation = Quaternion.Euler(0,0,-90);
        direction = Vector2.right;
        Time.timeScale = .1f;
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
        for (int i = 0; i < 2; i++) {
            Grow();
            Time.timeScale -= difficultyTime;
        }
    }

    void Grow () {
        GameObject newSegment = Instantiate(segment);
        newSegment.transform.position = segments[segments.Count - 1].transform.position; //the position is exactly where the old one currently is.
        segments.Add(newSegment);
        if (difficultyScale == "everett" && Time.timeScale <= .1f) {
            Time.timeScale += difficultyTime;
        }
        else if (difficultyScale == "basic" && Time.timeScale <= .05f){
            Time.timeScale += difficultyTime;
        } else if (Time.timeScale <= .25f) {
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
        } else if (Input.GetKeyDown(KeyCode.R)) {
            ResetSnake();
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
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
            updateHighScore();
        } else if (collide.CompareTag("Food")) {
            Grow();
            AddScore(1);
            updateHighScore();
            if (snakeScore == 10 && PlayerPrefs.GetInt("scoreEverett") != 1) {
                //Debug.Log("Yeah it happened");
                PlayerPrefs.SetInt("scoreEverett", 1);
                OnEverettUnlock.Invoke(true);
            }
            //Debug.Log("Score = "+snakeScore);
        }
    }
}
