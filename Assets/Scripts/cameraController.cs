using System;
using TMPro;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [Header("Player Settings")]
    public float camSensitivity;
    public float throwForce;
    public float pickupRange;
    public GameObject player;
    public Transform orientation;

    [Header("Ball Settings")]
    public Transform ball;
    public Transform respawn1;
    public Transform respawn2;
    public Transform respawn3;
    public Transform respawn4;
    public Transform holdPos;

    [Header("Game Settings")]
    public AudioSource bgMusic;
    public Transform pauseMenu;
    public TMP_Text timerText, pauseMenuText;
    public float timer;

    private float rotatX, rotatY, mousePosX, mousePosY;
    private GameObject ballObj;
    private Rigidbody ballRB;
    private bool isPaused, isGameOver;

    private void Start()
    {
        //Lock cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        isPaused = true;
        isGameOver = false;
    }

    void Update()
    {
        mousePosX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * camSensitivity;
        mousePosY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * camSensitivity;

        rotatY += mousePosX;
        rotatX -= mousePosY;
        rotatX = Mathf.Clamp(rotatX, -90f, 75f); //restrict camera movement

        transform.rotation = Quaternion.Euler(rotatX, rotatY, 0);
        orientation.rotation = Quaternion.Euler(0, rotatY, 0);

        checkIfPaused();
        updateTimer();
        if (ballObj != null)
        {
            moveBall();
        }
        getInputs();
    }

    private void getInputs()
    {
        //press mouse left click pick up or drop ball
        if (Input.GetMouseButtonDown(0))
        {
            if(ballObj == null)
            {
                RaycastHit rcHit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rcHit, pickupRange))
                {
                    if(rcHit.transform.gameObject.tag == "ball")
                    {
                        pickUpBall(rcHit.transform.gameObject);
                    }
                }
            }
            else
            {
                dropBall();
            }
        }

        //press mouse right click to throw ball
        if(ballObj != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                throwBall();
            }
        }
        
        //press R to reset ball position
        if (Input.GetKeyDown(KeyCode.R) && ballObj == null) resetBallPos();

        //press space to pause the game
        if (Input.GetKeyDown(KeyCode.Space) && !isGameOver)
        {
            if (isPaused) isPaused = false;
            else if (!isPaused) isPaused = true;
        }

        //press M to mute background music
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (bgMusic.mute == true) bgMusic.mute = false;
            else if (bgMusic.mute == false) bgMusic.mute = true;
        }
    }

    private void pickUpBall(GameObject ballInView)
    {
        if (ballInView.GetComponent<Rigidbody>())
        {
            ballObj = ballInView;
            ballRB = ballInView.GetComponent<Rigidbody>();
            ballRB.isKinematic = true;
            Physics.IgnoreCollision(ballObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    private void dropBall()
    {
        Physics.IgnoreCollision(ballObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        ballRB.isKinematic = false;
        ballObj = null;
    }

    private void moveBall()
    {
        ballRB.MovePosition(holdPos.position);
    }

    private void throwBall()
    {
        Physics.IgnoreCollision(ballObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        ballRB.isKinematic = false;
        ballRB.AddForce(transform.forward * throwForce);
        ballObj = null;
    }

    private void resetBallPos()
    {
        int pos = UnityEngine.Random.Range(1, 4);
        switch (pos)
        {
            case 1:
                ball.position = respawn1.position;
                break;
            case 2:
                ball.position = respawn2.position;
                break;
            case 3:
                ball.position = respawn3.position;
                break;
            case 4:
                ball.position = respawn4.position;
                break;
            default:
                ball.position = respawn1.position;
                break;

        }
    }

    private void checkIfPaused()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            bgMusic.UnPause();
            pauseMenu.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!isPaused)
        {
            Time.timeScale = 0;
            bgMusic.Pause();
            pauseMenu.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void gameOver()
    {
        pauseMenuText.SetText("G A M E   O V E R");
        isGameOver = true;
        isPaused = false;
    }

    private void updateTimer()
    {
        timer -= Time.deltaTime;
        TimeSpan clock = TimeSpan.FromSeconds(timer);
        timerText.SetText(clock.ToString("mm':'ss"));
        if (timer <= 0) gameOver();
    }
}
