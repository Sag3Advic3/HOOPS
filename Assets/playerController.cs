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

    private float inputX;
    private float inputY;

    private bool isHoldingBall = false;

    Vector3 moveDir;

    Rigidbody rb;
    Rigidbody basketballRB;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        basketballRB = ball.GetComponent<Rigidbody>();
    }

    void Update()
    {
        getInputs();

        //include drag so player movement is not slippery
        rb.linearDamping = groundDrag;

        //press left mouse click to throw the ball
        if (Input.GetMouseButtonDown(0) && isHoldingBall)
        {
            isHoldingBall = false;
            basketballRB.freezeRotation = false;
            basketballRB.linearVelocity = playerCam.forward * throwingSpeed * Time.deltaTime;
        }

        if (isHoldingBall)
        {
            ball.position = holdBallPos.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //when colliding with the ball, pick it up
        if (collision.gameObject.tag == "ball" && !isHoldingBall)
        {
            isHoldingBall = true;
            basketballRB.freezeRotation = true;
        }

    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void getInputs()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void movePlayer()
    {
        moveDir = (orientation.forward * inputY) + (orientation.right * inputX);
        rb.AddForce(moveDir.normalized * playerSpeed * 10f, ForceMode.Force);
    }
}
