using UnityEngine;

public class PlayerplayerController : MonoBehaviour
{
    public float moveSpeed = 5f; public float jumpForce = 5f; public float gravityScale = -9.81f;
    private CharacterController playerController;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        if (playerController == null)
        {
            Debug.LogError("CharacterplayerController не найден на объекте!");
        }
    }

    void Update()
    {
        isGrounded = playerController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        float moveX = Input.GetAxis("Horizontal"); float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        playerController.Move(move * moveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1));

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravityScale);
        }

        velocity.y += gravityScale * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }
}