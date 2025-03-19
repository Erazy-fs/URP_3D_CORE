using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Amount = 1;
    public float Speed = 1;
    public float SensVert = 9;

    public float MinVert = -45;
    public float MaxVert = 45;


    Vector3 startPos;
    float distantion = 0;
    float rotationX = 0;
    Vector3 rotation = Vector3.zero;

    public Transform playerBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //startPos = transform.position;
        //Debug.Log(transform);
    }

    // Update is called once per frame
    void Update()
    {
        //rotationX -= Input.GetAxis("Mouse X") * SensVert;
        //rotationX = Mathf.Clamp(rotationX, MinVert, MaxVert);

        //distantion -= (transform.position - startPos).magnitude;
        //startPos = transform.position;
        //rotation.z = Mathf.Sin(distantion * Speed) * Amount;
        //transform.localEulerAngles = new Vector3(rotation.x, transform.localEulerAngles.y, rotation.z);
        float mouseX = Input.GetAxis("Mouse X") * SensVert * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * SensVert * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
