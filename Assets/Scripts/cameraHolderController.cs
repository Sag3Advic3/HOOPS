using UnityEngine;

public class cameraHolderController : MonoBehaviour
{
    public Transform cameraPos;

    void Update()
    {
        //camera follow position marker
        transform.position = cameraPos.position;
    }
}
