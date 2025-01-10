using UnityEngine;

public class cameraController : MonoBehaviour
{
    public float camSensitivity;

    public Transform orientation;

    private float rotatX;
    private float rotatY;

    private void Start()
    {
        //Lock cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mousePosX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * camSensitivity;
        float mousePosY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * camSensitivity;

        rotatY += mousePosX;
        rotatX -= mousePosY;
        rotatX = Mathf.Clamp(rotatX, -90f, 90f); //restrict camera movement

        transform.rotation = Quaternion.Euler(rotatX, rotatY, 0);
        orientation.rotation = Quaternion.Euler(0, rotatY, 0);
    }
}
