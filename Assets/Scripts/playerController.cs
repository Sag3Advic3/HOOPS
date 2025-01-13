using UnityEngine;

public class playerController : MonoBehaviour
{
    public float playerSpeed, groundDrag;
    public Transform orientation;

    private float inputX, inputY;

    private Vector3 moveDir;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        //include drag so player movement is not slippery
        rb.linearDamping = groundDrag;
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        moveDir = (orientation.forward * inputY) + (orientation.right * inputX);
        rb.AddForce(moveDir.normalized * playerSpeed * 10f, ForceMode.Force);
    }
}
