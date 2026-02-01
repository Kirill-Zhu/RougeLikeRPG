using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCahracterController : MonoBehaviour {

    [SerializeField] InputReader input;
    [SerializeField] CharacterController controller;
    [Header("Move Settings")]
    [SerializeField] float speed = 4;
    [Header("Jumping settings")]
    [SerializeField] float jumpTime = 1;
    float jumpTimer;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float landingSpeed = -3f;
    public bool IsJumping { get; private set; }
    Vector2 direction;
    Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
        controller = GetComponent<CharacterController>();
    }
    private void Start() {
        input.Move += moveInput => direction = moveInput;
        input.Jump += isJumpButtonPressd => {
            switch (isJumpButtonPressd) {
                case true:
                    HandleJumping();
                    break;
                case false:
                    Land();
                    break;
            }
        };
        input.EnablePlayerActions();
    }
    public void HandleMovement() {
        Move(CalculateDirection());
    }
    public void HandleLanding() {
        Move(CalculateDirection(), landingSpeed);
    }
    public void Move(Vector3 direction, float withGravity = -1f) {
        if (direction.sqrMagnitude > 0.01f) {
            transform.rotation = Quaternion.LookRotation(direction);
            //transform.position += direction * Time.deltaTime;
            direction *= speed;
            direction.y += withGravity;
            controller.Move(direction * Time.deltaTime);
        } else {
            controller.Move(new Vector3(0, withGravity, 0) * Time.deltaTime);
        }
    }
    private Vector3 CalculateDirection() {

        Vector3 cameraForward = mainCamera.transform.forward.WithY(0);
        Vector3 cameraRight = mainCamera.transform.right.WithY(0);
        Vector3 dir = cameraForward * direction.y + cameraRight * direction.x;
        return dir;
    }
    public void RefreshJumpTimer() {
        jumpTimer = jumpTime;
    }
    public void HandleJumping() {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0) {
            IsJumping = false;
            return;
        }
        IsJumping = true;

        Move(CalculateDirection(), jumpForce);
        //Debug.Log("Jump");
    }
    public void Land() {
        IsJumping = false;
        Debug.Log("Land");
    }
    public bool Grounded() {
        return controller.isGrounded;
        // return Physics.CheckSphere(transform.position, 1, 1 << LayerMask.NameToLayer("Default"));
    }
    private void OnDrawGizmosSelected() {
        //Ground Check
        if (Grounded()) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}
