using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speedScrollCamera = 1;
    public Transform targetObj;
    public Transform weapon;
    private float mouseSens = 4f;
    private float smooth = 2f;
    private float minimumY = -90f;
    private float maximumY = 360f;
    private float minimumX = -90f;
    private float maximumX = 180f;
    private float minimumZ = -90f;
    private float maximumZ = 90f;
    private Transform character;
    private Vector2 currentMouseLook; 
    private Vector2 appliedMouseDelta;
    private float rotationX = 1;
    private float rotationY = 1;
    private float cameraPositionZ = 1;
    private float tmpPositionY = 2;
    private Vector3 tempPositionBot;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        rotationX += (Input.GetAxis("Mouse X") * (mouseSens));
        rotationY -= (Input.GetAxis("Mouse Y") * (mouseSens));
        cameraPositionZ = Mathf.Clamp(cameraPositionZ + (Input.GetAxis("Mouse ScrollWheel") * speedScrollCamera), minimumZ, maximumZ);

        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        tempPositionBot = new Vector3(targetObj.transform.position.x,
            targetObj.transform.position.y + tmpPositionY,
            targetObj.transform.position.z);

        float tmpPositionZ = cameraPositionZ * 0.1f;

        var newRotation = Quaternion.Euler(rotationY, rotationX, 0f);
        var newPosition = newRotation * new Vector3(0f, 0f, tmpPositionZ) + tempPositionBot;

        transform.rotation = newRotation;
        transform.position = newPosition;
        //weapon.rotation = newRotation;
    }
}
