using UnityEngine;
using TMPro;

public class ballController : MonoBehaviour
{
    public TMP_Text scoreText, scorePauseText, highScoreText;
    public AudioSource swish;
    public AudioSource highScoreAudio;
    public GameObject scorePoint;
    public int startingHighScore;

    private int scoreCount;
    private string highScore = "highScore";
    private bool isFalling;

    void Start()
    {
        scoreCount = 0;
        if(PlayerPrefs.HasKey(highScore)) highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
        else
        {
            //preset high score
            PlayerPrefs.SetInt(highScore, startingHighScore);
            PlayerPrefs.Save();
            highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
        }
    }

    void Update()
    {
        isFalling = GetComponent<Rigidbody>().linearVelocity.y < 0 ? true : false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //when colliding with the score trigger point and is falling, increase score and check high score value
        //isFalling is used so the player cannot score from the underside of the hoop
        if (collision.gameObject == scorePoint && isFalling)
        {
            scoreCount++;
            scoreText.SetText(scoreCount.ToString());
            scorePauseText.SetText(scoreCount.ToString());
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
                highScoreAudio.Play();
            }
            else swish.Play();
        }
        else
        {
            PlayerPrefs.SetInt(highScore, scoreCount);
            PlayerPrefs.Save();
            highScoreText.SetText(PlayerPrefs.GetInt(highScore).ToString());
            highScoreAudio.Play();
        }
    }
}
