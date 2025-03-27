using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class TopDownControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public float gravityScale = 2f;
    public float rotationSpeed = 10f;
    public GroundControl groundControl;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Animator animator;
    private GunControl gun;
    private Rigidbody rigitbody;

    public bool canMove = true;
    public bool canShoot = true;
    public bool canCallZEUS = true;

    public LevelNarrator narrator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gun = GetComponentInChildren<GunControl>();
        rigitbody = GetComponent<Rigidbody>();
    }

    private bool fireModeIsAuto = false;
    void Update()
    {

        if (transform.position.y < -1)
        {
            narrator.PlayerDeath();
        }
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        var leftButton = Input.GetMouseButton(0);
        var rightButton = Input.GetMouseButton(1);

        if (canMove)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            var move = new Vector3(moveHorizontal, 0, moveVertical);

            if (move != Vector3.zero)
            {
                if (leftButton || rightButton)
                {
                    controller.Move(move * moveSpeed * Time.deltaTime);
                }
                else
                {
                    var toRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.z), Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                    controller.Move(transform.forward * moveSpeed * Time.deltaTime);
                }
                animator.SetBool("isWalking", true);
            }
            else animator.SetBool("isWalking", false);

            if (Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y * gravityScale);
        }
        else LookAtMouse();


        if (canShoot)
        {
            if (leftButton || rightButton)
            {
                LookAtMouse();
                var wait = !animator.GetBool("isHolding");
                animator.SetBool("isHolding", true);
                gun.Shoot(wait, rightButton);
            }
            else
            {
                animator.SetBool("isHolding", false);
                gun.StopShooting();
            }
        }

        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);


        //Z.E.U.S.
        if (canCallZEUS)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, 3))
                {
                    var obj = hit.collider.gameObject;
                    var plot = obj.GetComponent<PlotControl>();
                    if (plot is not null)
                        if (!plot.isComplete)
                            groundControl.CallInZEUS(hit.point, plot.colorIndex);
                }
            }
        }
    }

    private void LookAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;

        var ray = Camera.main.ScreenPointToRay(mousePosition);
        var groundPlane = new Plane(Vector3.up, transform.position);
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 direction = ray.GetPoint(distance) - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
            }
        }
    }

}