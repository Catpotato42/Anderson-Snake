using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    public void updateHighScore (string highScore) {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        scoreText.text = PlayerPrefs.GetInt(highScore).ToString();
    }
}
