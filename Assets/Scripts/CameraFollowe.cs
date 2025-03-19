using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player to follow
    public Transform player;
    // Offset between the camera and the player
    public Vector3 offset = new Vector3(0, 10, -10);
    // Smoothness of the camera follow
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;
        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Set the camera's position
        transform.position = smoothedPosition;

        // Look at the player
        transform.LookAt(player);
    }
}