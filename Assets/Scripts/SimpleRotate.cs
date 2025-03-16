using UnityEngine;

public class SimpleRotate : MonoBehaviour
{

    public float xAngle;
    public float yAngle;
    public float zAngle;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access player's Rigidbody.
        rb.AddTorque(xAngle, yAngle, zAngle, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
