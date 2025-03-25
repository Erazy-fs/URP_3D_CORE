using UnityEngine;

public class Move : MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 300f;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 movedirection;

    private bool _isGrounded;
    private Rigidbody _rb;
    public Transform _camera;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MovementLogic();
        JumpLogic();
    }

    private void MovementLogic()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 rightDirection = _camera.right;
            if (horizontalInput > 0)
            {
                rightDirection = _camera.right;
            }
            else if (horizontalInput < 0)
            {
                rightDirection = -_camera.right;
            }
            else if (verticalInput > 0)
            {
                rightDirection = _camera.forward;
            }
            else if (verticalInput < 0)
            {
                rightDirection = -_camera.forward;
            }
            
            Vector3 newDirection = new Vector3(rightDirection.x, 0, rightDirection.z).normalized;
            Quaternion newRotation = Quaternion.LookRotation(newDirection, Vector3.up);
            transform.rotation = newRotation;
            _rb.AddForce(transform.forward * Speed);
        }
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * JumpForce);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }
    public float rotationSpeed = 10f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 targetDir = new Vector3(0, 0, 0) - transform.position;
            float angle = Vector3.Angle(transform.forward, targetDir);
            Vector3 cross = Vector3.Cross(transform.forward, targetDir);
            if (cross.y < 0) angle = -angle;
            transform.Rotate(0, angle, 0, Space.World);
        }
    }
}
