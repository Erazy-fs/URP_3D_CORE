using UnityEngine;

public class DownControl : MonoBehaviour{
    public float moveSpeed     = 5f;
    public float jumpForce     = 2f;
    public float gravityScale  = 2f;
    public float rotationSpeed = 10f;
    public GroundControl groundControl;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Animator animator;
    private GunControl gun;
    

    void Start() {
        controller = GetComponent<CharacterController>();
        animator   = GetComponent<Animator>();
        gun        = GetComponentInChildren<GunControl>();
    }

    private bool fireModeIsAuto = false;
    void Update() {

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0) 
            velocity.y = -2f;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical   = Input.GetAxis("Vertical");

        var move = new Vector3(moveHorizontal, 0, moveVertical);

        if (move != Vector3.zero) {
            var toRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.z), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            controller.Move(transform.forward * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else animator.SetBool("isWalking", false);

        // if (Input.GetButtonDown("Jump") && isGrounded)
        //     velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y * gravityScale);
        // var t = Time.deltaTime;
        // var currentFiring = animator.GetLayerWeight(1);
        // if (Input.GetKey(KeyCode.Space)) 
        //     animator.SetLayerWeight(1, Mathf.Clamp01(currentFiring+t+.1f));
        // else 
        //     animator.SetLayerWeight(1, Mathf.Clamp01(currentFiring-t+.05f));
        if (Input.GetKeyDown(KeyCode.V)){
            fireModeIsAuto = !fireModeIsAuto;
            Debug.Log("mode: " + (fireModeIsAuto?"auto":"semi-auto"));
        }

        if (Input.GetKey(KeyCode.Space)) {
            var wait = !animator.GetBool("isHolding");
            animator.SetBool("isHolding", true);
            gun.Shoot(wait, fireModeIsAuto);
        } else {
            animator.SetBool("isHolding", false);
            gun.StopShooting();
        }

        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);


        //Z.E.U.S.
        if (Input.GetKeyDown(KeyCode.F)){

            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 2)) {
                var obj = hit.collider.gameObject;
                Debug.Log("object: " + obj.name);
                var plot = obj.GetComponent<PlotControl>();
                if (plot is not null)
                    groundControl.CallInZEUS(hit.point, plot.colorIndex);
            } 
        }
    }

}