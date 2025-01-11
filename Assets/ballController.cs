using UnityEngine;
using System;
using TMPro;

public class ballController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text highScoreText;
    public GameObject tipText;
    public AudioSource swish;

    private int scoreCount;
    private float timer;
    private string highScore = "highScore";
    private Vector3 startPos;

    void Start()
    {
        scoreCount = 0;
        tipText.SetActive(false);
        startPos = transform.position;
        if(PlayerPrefs.HasKey(highScore)) highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
    }

    void Update()
    {
        timer += Time.deltaTime;
        TimeSpan clock = TimeSpan.FromSeconds(timer);
        timerText.SetText(clock.ToString("mm':'ss"));

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = startPos;
        }

        //Show tip after 15 seconds
        if (clock.TotalSeconds > 15)
        {
            tipText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //when colliding with the score trigger point, increase score and check high score value
        if (collision.gameObject.tag == "score")
        {
            scoreCount++;
            scoreText.SetText(scoreCount.ToString());
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
