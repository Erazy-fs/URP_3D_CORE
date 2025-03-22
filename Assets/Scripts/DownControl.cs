using UnityEngine;

public class DownControl : MonoBehaviour{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravityScale = 1f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Animator animator;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Reset downward velocity when grounded

        // Get input from the user
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical   = Input.GetAxis("Vertical");

        // Create a movement vector
        var move = new Vector3(moveHorizontal, 0, moveVertical);

        // Move the player
        if (move != Vector3.zero) {
            // Rotate the player to face the direction of movement
            RotatePlayer(move);

            // Move the player forward in the direction they are facing
            controller.Move(transform.forward * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else animator.SetBool("isWalking", false);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y * gravityScale);

        // Apply gravity
        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    void RotatePlayer(Vector3 move) {
        // Calculate the target rotation
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.z), Vector3.up);

        // Smoothly rotate the player towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}