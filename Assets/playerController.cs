using System;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float playerSpeed;
    public float throwingSpeed;
    public float groundDrag;
    public Transform orientation;
    public Transform ball;
    public Transform holdBallPos;
    public Transform playerCam;
    public Transform pauseMenu;
    public AudioSource bgMusic;

    private float inputX;
    private float inputY;
    private bool isPaused;

    private bool isHoldingBall = false;

    Vector3 moveDir;

    Rigidbody rb;
    Rigidbody basketballRB;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        basketballRB = ball.GetComponent<Rigidbody>();
        isPaused = true;
        pauseMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        getInputs();
        checkIfPaused();

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

    private void checkIfPaused()
    {
            if (isPaused)
            {
                Time.timeScale = 1;
                bgMusic.UnPause();
                pauseMenu.gameObject.SetActive(false);
                Cursor.visible = false;
            }
            else if (!isPaused)
            {
                Time.timeScale = 0;
                bgMusic.Pause();
                pauseMenu.gameObject.SetActive(true);
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

        //press space to pause the game
        if (Input.GetKeyDown(KeyCode.Space))
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
}
