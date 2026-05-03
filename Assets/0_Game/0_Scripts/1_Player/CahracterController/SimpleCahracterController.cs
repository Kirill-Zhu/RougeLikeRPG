using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IUpgradable {
    public void ClearItems();
    public void AddItem(Item item);
    public void RefreshItemUpgrades();
}
[RequireComponent(typeof(SimpleCahracterController))]
public class SimpleCahracterController : MonoBehaviour, IVisitable, IUpgradable {

    [SerializeField] CharacterControllerData data;
    [SerializeField] InputReader input;
    [SerializeField] CharacterController controller;
    [Header("Move Settings")]
    float startSpeed;
    [SerializeField] float speed = 4;
    [Header("Jumping settings")]
    [SerializeField] float jumpTime = 1;
    float jumpTimer;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float landingSpeed = -3f;
    public bool IsJumping { get; private set; }
    public Vector3 InputDirection => moveDirection;
    Vector2 moveDirection;
    Camera mainCamera;
    #region ANDROID
    DynamicJoystick joystick;

    //Upgrades 
    List<MoveItem> itemsList = new List<MoveItem>();
    public void SetUpAndoridJoystick(DynamicJoystick joystick) {
        this.joystick = joystick;
    }
    #endregion
    public void Initialize(CharacterControllerData data) {
        this.data = data;
    }
    void SubscribeAndroidInputs(PointerEventData eventData) {
        moveDirection = joystick.Direction;
    }
    private void Awake() {
        mainCamera = Camera.main;
        controller = GetComponent<CharacterController>();

        startSpeed = speed;
    }
    private void Start() {
        //Listen Events
        input.Move += SubscribeMoveInputs;
        input.Jump += SubscribeJumpingButton;
        input.EnablePlayerActions();


        //Android
        joystick.OnMoveJoystick.AddListener((dir) => moveDirection = dir);
        joystick.OnMoveJoystick.AddListener((_) => HandleMovement());
        joystick.OnPointerUpEvent.AddListener(() => moveDirection = Vector2.zero);
    }
    //TestUpdate
    private void Update() {
        if (input.Direction == Vector3.zero)
            moveDirection = joystick.Direction;
    }
    void SubscribeJumpingButton(bool isJumpButtonPressd) {
        switch (isJumpButtonPressd) {
            case true:
                HandleJumping();
                break;
            case false:
                Land();
                break;
        }
    }

    void SubscribeMoveInputs(Vector2 moveInput) {
        moveDirection = moveInput;
    }
    public void HandleMovement() {
        Move(CalculateDirection());
    }
    public void HandleInFightMovement() {                                          //Use it to look in direction in fight
        Move(CalculateDirection(), -1f, 0.1f);                //Use
    }
    public void HandleLanding() {
        Move(CalculateDirection(), landingSpeed);
    }
    public void Move(Vector3 direction, float withGravity = -1f, float speedModifier = 1) {
        if (direction.sqrMagnitude > 0.01f) {
            transform.rotation = Quaternion.LookRotation(direction);
            //transform.position += direction * Time.deltaTime;
            direction *= speed * speedModifier;
            direction.y += withGravity;
            controller.Move(direction * Time.deltaTime);
        } else {
            controller.Move(new Vector3(0, withGravity, 0) * Time.deltaTime);
        }
    }
    private Vector3 CalculateDirection() {
        if (mainCamera == null) { mainCamera = Camera.main; }
        Vector3 cameraForward = mainCamera.transform.forward.WithY(0);
        Vector3 cameraRight = mainCamera.transform.right.WithY(0);
        Vector3 dir = cameraForward * moveDirection.y + cameraRight * moveDirection.x;
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
            Gizmos.DrawSphere(transform.position + new Vector3(0, -0.5f, 0), 0.1f);
        }
    }

    private void OnDestroy() {
        input.Move -= SubscribeMoveInputs;
        input.Jump -= SubscribeJumpingButton;
    }
    #region UPGRADE STRATEGY

    public void ClearItems() {
        itemsList.Clear();
    }
    public void AddItem(Item item) {
        if(item is MoveItem)
            itemsList.Add(item as MoveItem);    

    }
    public void RefreshItemUpgrades() {
        speed = startSpeed;
        foreach (MoveItem item in itemsList) {
            speed += item.AddMoveSpeed;
        }
    }
    
    #endregion
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
}
