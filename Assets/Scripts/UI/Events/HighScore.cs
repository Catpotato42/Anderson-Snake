using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour, ISaveManager
{
    //It might be more efficient to store high score in game manager, 
    //as it would be easier to have it loaded in the title screen and 
    //put into a static variable so I don't have to update it in countdown visibly after the game starts.
    private int highScoreB = 0;
    private int highScoreM = 0;
    private int highScoreH = 0;
    private int highScoreEv = 0;
    public void SaveData (GameData data) {
        data.highScoreB = highScoreB;
        data.highScoreM = highScoreM;
        data.highScoreH = highScoreH;
        data.highScoreEv = highScoreEv;
    }
    public void LoadData (GameData data) {
        highScoreB = data.highScoreB;
        highScoreM = data.highScoreM;
        highScoreH = data.highScoreH;
        highScoreEv = data.highScoreEv;
    }
    private TextMeshProUGUI scoreText;
    public void UpdateHighScore (string difficulty, int newHighScore) {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        switch(difficulty) {
            case "basic":
                highScoreB = newHighScore;
                scoreText.text = highScoreB.ToString();
                break;
            case "medium":
                highScoreM = newHighScore;
                scoreText.text = highScoreM.ToString();
                break;
            case "hard":
                highScoreH = newHighScore;
                scoreText.text = highScoreH.ToString();
                break;
            case "everett":
                highScoreEv = newHighScore;
                scoreText.text = highScoreEv.ToString();
                break;
            default:
                highScoreB = -1;
                scoreText.text = highScoreB.ToString();
                break;
        }
    }

    public void UpdateHighScore (string difficulty) {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        switch(difficulty) {
            case "basic":
                scoreText.text = highScoreB.ToString();
                break;
            case "medium":
                scoreText.text = highScoreM.ToString();
                break;
            case "hard":
                scoreText.text = highScoreH.ToString();
                break;
            case "everett":
                scoreText.text = highScoreEv.ToString();
                break;
            default:
                highScoreB = -1;
                scoreText.text = highScoreB.ToString();
                break;
        }
    }

    public int GetHighScore (string difficulty) {
        switch(difficulty) {
            case "basic":
                return highScoreB;
            case "medium":
                return highScoreM;
            case "hard":
                return highScoreH;
            case "everett":
                return highScoreEv;
            default:
                return -1;
        }
    }
}
