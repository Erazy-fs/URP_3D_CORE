using System.Diagnostics;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 300f;

    //��� �� ��� ���������� �������� �������� ��� "Ground" �� ���� ����������� �����
    private bool _isGrounded;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // �������� �������� ��� ��� �������� � ������� 
    // ���������� ������������ � FixedUpdate, � �� � Update
    void FixedUpdate()
    {
        MovementLogic();
        JumpLogic();
    }

    private void MovementLogic()
    {
        if (_isGrounded)
        { 
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            _rb.AddForce(movement * Speed);
        }
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            //Debug.Log("Jump");
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * JumpForce);

                // �������� �������� ��� � ����� �� ������ Vector3.up 
                // � �� �� ������ transform.up. ���� �������� ���� ��� 
                // ���� �������� -- ���, �� ��� ������ "����" ����� 
                // ����� �����������. �����, ������, ����...
                // �� ��� ����� ������ ������ � ���������� �����, 
                // ������ � Vector3.up
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
            //Debug.Log("Collide");
            _isGrounded = value;
        }
    }
}
