using UnityEngine;
using System;
using TMPro;

public class ballController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text scorePauseText;
    public TMP_Text highScoreText;
    public AudioSource swish;

    private int scoreCount;
    private string highScore = "highScore";
    private bool isFalling;

    void Start()
    {
        scoreCount = 0;
        if(PlayerPrefs.HasKey(highScore)) highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
    }

    void Update()
    {
        isFalling = GetComponent<Rigidbody>().angularVelocity.y < 0 ? true : false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //when colliding with the score trigger point and is falling, increase score and check high score value
        //isFalling is used so the player cannot score from the underside of the hoop
        if (collision.gameObject.tag == "score" && isFalling)
        {
            scoreCount++;
            scoreText.SetText(scoreCount.ToString());
            scorePauseText.SetText(scoreCount.ToString());
            swish.Play();
            checkHighScore();
        }

    }

    private void checkHighScore()
    {
        if (PlayerPrefs.HasKey(highScore))
        {
            if(scoreCount > PlayerPrefs.GetInt(highScore))
            {
                PlayerPrefs.SetInt(highScore, scoreCount);
                PlayerPrefs.Save();
                highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
            }
        }
        else
        {
            PlayerPrefs.SetInt(highScore, scoreCount);
            PlayerPrefs.Save();
            highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
        }
    }
}
