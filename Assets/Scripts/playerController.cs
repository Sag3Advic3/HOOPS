using System;
using TMPro;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float playerSpeed;
    public float throwingSpeed;
    public float groundDrag;
    public float timer;
    public Transform orientation;
    public Transform ball;
    public Transform holdBallPos;
    public Transform playerCam;
    public Transform pauseMenu;
    public AudioSource bgMusic;
    public TMP_Text timerText;
    public TMP_Text pauseMenuText;

    private float inputX;
    private float inputY;
    private bool isPaused;
    private bool isGameOver;
    private Vector3 ballStartPos;

    private bool isHoldingBall = false;

    Vector3 moveDir;

    Rigidbody rb;
    Rigidbody basketballRB;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        basketballRB = ball.GetComponent<Rigidbody>();
        ballStartPos = ball.position;
        isPaused = true;
        isGameOver = false;
        pauseMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        getInputs();
        checkIfPaused();
        updateTimer();

        //include drag so player movement is not slippery
        rb.linearDamping = groundDrag;

        if (isHoldingBall)
        {
            ball.position = holdBallPos.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //when colliding with the ball, pick it up
        if (collision.gameObject.tag == "ball")
        {
            if (isHoldingBall)
            {
                //This addresses a bug where the two colliders meetin pushes the player backward
                Physics.IgnoreCollision(GetComponent<Collider>(), ball.GetComponent<Collider>());
            }
            else
            {
                isHoldingBall = true;
                basketballRB.freezeRotation = true;
            }
        }

    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void updateTimer()
    {
        timer -= Time.deltaTime;
        TimeSpan clock = TimeSpan.FromSeconds(timer);
        timerText.SetText(clock.ToString("mm':'ss"));
        if (timer <= 0) gameOver();
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


    private void getInputs()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        //press left mouse click to throw the ball
        if (Input.GetMouseButtonDown(0) && isHoldingBall)
        {
            isHoldingBall = false;
            basketballRB.freezeRotation = false;
            basketballRB.linearVelocity = playerCam.forward * throwingSpeed * Time.deltaTime;
        }

        //press mouse right click to put ball in hand
        if (Input.GetMouseButtonDown(1))
        {
            isHoldingBall = true;
            basketballRB.freezeRotation = true;
        }

        //press space to pause the game
        if (Input.GetKeyDown(KeyCode.Space) && !isGameOver)
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else if (!isPaused)
            {
                isPaused = true;
            }
        }
    }

    private void movePlayer()
    {
        moveDir = (orientation.forward * inputY) + (orientation.right * inputX);
        rb.AddForce(moveDir.normalized * playerSpeed * 10f, ForceMode.Force);
    }

    public void gameOver()
    {
        pauseMenuText.SetText("G A M E   O V E R");
        isGameOver = true;
        isPaused = false;
    }
}
