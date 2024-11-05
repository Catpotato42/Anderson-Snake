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


    private string[] noScore = {"WASD or arrow keys to move!", "AFK?"};
    private string[] deathMessages0 = {"ouch", "didn't see that wall?", "you matter", "try again?", "unscaled, unpolished, can't slither.", "python"};
    private string[] deathMessages1 = {"mmmmmmm", "getting along now", "gears turning, tongue hissing", "doin it", "alright", "common garter snake", "peasant snake"};
    private string[] deathMessages2 = {"ok!", "Bonk","great!", "not not great", "be a snake, be happy", "pixelated snake, real feelings", "worker snake"};
    private string[] deathMessages3 = {"cookin", "slithered fr", "ssssss", "your name starts with an s", "above average snake", "viper", "skilled laborer snake"};
    private string[] deathMessages4 = {"i'm just ssstraight up impressed", "91% of statissstics are made up", "you have free will go make a cardboard cutout of somebody", "snakes aren't just scaly worms. they have eyes and they also probably have more hearts than worms do unconfirmed", "fantastic mr snake", "noble snake"};
    private string[] deathMessages5 = {"the reptile government wants to hire you", "can I get a sssselfie?", "this would go viral on snake social media", "you're like if in metamorphosis the guy turned into a snake instead and everybody loved him too", "you should dress up as a snake next halloween (subliminal messaging)", "you're like that solid guy from metal gear i forgot his name", "imperator snake"};
    private string[] deathMessages6 = {"◪_◪"};
    private string[] deathMessages7 = {"less than 1.91415% of snakes make it here", "turn on the ac cause that was some heat", "mute this music and put on some clifford brown. you deserve it.","monarch snake"};
    private string[] deathMessages8 = {"if the beatles were beetles and arctic monkey were actual arctic monkeys you would start the pythons and win 30 million grammys", "mice faint just at the sight of you", "hyper-lethal killer", "potentate of the snakes", "it's lonely at the top"};

    public void Setup (int score) {
        gameObject.SetActive(true);
        if (finalScore != null) {
            finalScore.text = "score: "+score.ToString();
        }
        deathTextGen(score);
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
