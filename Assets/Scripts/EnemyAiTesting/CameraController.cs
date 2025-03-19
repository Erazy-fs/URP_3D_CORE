using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 2f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        player.Rotate(Vector3.up * mouseX);
    }
}