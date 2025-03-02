using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private TextMeshProUGUI coinsText;


    private string[] noScore = {"WASD or arrow keys to move!", "AFK?"};
    private string[] deathMessages0 = {"the snake language doesn't have a word for quit", "didn't see that wall?", "you matter to a snake somewhere", "unscaled, unpolished, can't slither."};
    private string[] deathMessages1 = {"personal question: would you rather have scales?", "what is a snake?", "are snakes happy or sad most of the time?", "I can relate to a snake"};
    private string[] deathMessages2 = { "how do you feel about pythons vs vipers?", "pixelated snake, real feelings", "snakes are the only animal that probably couldn't hold a job if they gained human intelligence."};
    private string[] deathMessages3 = {"Any snakeologists out there? (that's the real name)", "slithered fr", "your name starts with an s", "I guess I should give you a real snake fact: they will never enjoy the feeling of a sunset run on a mountain trail. :("};
    private string[] deathMessages4 = {"91% of statissstics are made up", "have you ever seen a snake cry?", "fantastic mr snake", "it's bad etiquette to shake hands with a snake. You gotta go for a dap or a fist bump."};
    private string[] deathMessages5 = {"the reptile government wants to hire you", "the snake language doesn't have a word for winner unfortunately", "this would go viral on snake social media", "you're like if in metamorphosis the guy turned into a snake instead and everybody loved him too", "you should dress up as a snake next halloween (subliminal messaging)", "you're like that solid guy from metal gear i forgot his name", "imperator snake"};
    private string[] deathMessages6 = {"⌐■_■"};
    private string[] deathMessages7 = {"less than 1.91415% of snakes make it here", "What operating system would a snake use?", "snakes are often misunderstood in fiction they just want to bite things"};
    private string[] deathMessages8 = {"if the beatles were beetles and the arctic monkeys were actual arctic monkeys you would start the pythons and win 30 million grammys", "mice faint just at the sight of you", "snakes deserve more"};

    public void Setup (int score, int coinsMade) {
        gameObject.SetActive(true);
        if (finalScore != null) {
            finalScore.text = "score: "+score.ToString();
        }
        deathTextGen(score);
        coinsText.text = "Coins made: "+coinsMade;
    }

    private void deathTextGen (int score) {
        if (deathText == null) {
            return;
        }
        if (score == 0) {
            deathText.text = noScore[Random.Range(0, noScore.Length)]; //RANDOM.RANGE IS MAX EXCLUSIVE
        } else if (score < 9 && score > 0) {
            //0
            deathText.text = deathMessages0[Random.Range(0, deathMessages0.Length)];
        } else if (score >= 9 && score < 18) {
            //1
            deathText.text = deathMessages1[Random.Range(0, deathMessages1.Length)];
        } else if (score >= 18 && score < 36) {
            //2
            deathText.text = deathMessages2[Random.Range(0, deathMessages2.Length)];
        } else if (score >= 36 && score < 75) {
            //3
            deathText.text = deathMessages3[Random.Range(0, deathMessages3.Length)];
        } else if (score >= 75 && score < 150) {
            //4
            deathText.text = deathMessages4[Random.Range(0, deathMessages4.Length)];
        } else if (score >= 150 && score < 420) {
            //5
            deathText.text = deathMessages5[Random.Range(0, deathMessages5.Length)];
        } else if (score == 420) {
            //6
            deathText.text = deathMessages6[Random.Range(0, deathMessages6.Length)];
        } else if (score > 420 && score < 500) {
            //7
            deathText.text = deathMessages7[Random.Range(0, deathMessages7.Length)];
        } else if (score >= 500) {
            //8
            deathText.text = deathMessages8[Random.Range(0, deathMessages8.Length)];
        } else {
            deathText.text = "I'm not sure how we got here.";
        }
    }
}
