using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;
    
    private Camera cameraComponent;
    public float zoomSpeed = 5f;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 20f;

    void Start()
    {
        // Get the camera component attached to this GameObject
        cameraComponent = GetComponent<Camera>();

        // Ensure the camera is orthographic
        if (!cameraComponent.orthographic)
        {
            Debug.LogError("The camera is not orthographic. Please set it to orthographic in the Inspector.");
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(player);
        
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newOrthographicSize = cameraComponent.orthographicSize - scrollInput * zoomSpeed;
        newOrthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);
        cameraComponent.orthographicSize = newOrthographicSize;
    }
}