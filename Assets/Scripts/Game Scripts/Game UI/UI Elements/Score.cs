using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject player;
    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        Player playerScript = player.GetComponent<Player>();
        playerScript.OnScoreChanged += UpdateScoreText;
    }

    private void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
