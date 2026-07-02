using UnityEngine;

namespace CheeseChase {

    public class HeroMoveController : MonoBehaviour {
        [SerializeField] InputReader input;
        [SerializeField] Vector3 centerOfMassOffset;
        [SerializeField] float speed = 1;

        Vector2 inputDirection;
        Camera mainCamera;
        private void Awake() {

            input.Move += SubscribeMoveInputs;
            input.Jump += SubscribeJumpInput;
            input.EnablePlayerActions();

        }
        void SubscribeMoveInputs(Vector2 direction) {
            inputDirection = direction;
        }
        #region Physics


        public float torqueAmount = 2000f;
        public float rotationTorque = 100;
        public float maxAngularVelocity = 50f;
        public bool isJumping;
        public float JumpForce = 3;
        public float RotForce = 100;
        float jumpDuration;
        float jumpTimer;
        private Rigidbody rb;
     
        void Start() {
            rb = GetComponent<Rigidbody>();
            // Increase the maximum turning/rolling speed
            rb.maxAngularVelocity = maxAngularVelocity;
            rb.centerOfMass = centerOfMassOffset;
        }

        void FixedUpdate() {
            
            float moveHorizontal = inputDirection.x;
            float moveVertical = inputDirection.y;

           
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 forceDirection = (cameraForward * moveVertical + cameraRight * moveHorizontal).normalized;

            float rotationHandler = 0f;

            if (Vector3.Dot(cameraRight, transform.right) > 0f) {
                rotationHandler = 1;
            } else {
                rotationHandler = -1;
            }
            // Apply torque (rotation) to simulate rolling
            if (forceDirection != Vector3.zero) {
                Vector3 torqueAxis = Vector3.Cross(transform.up, cameraForward);
                rb.AddTorque(transform.right*rotationHandler* inputDirection.y * torqueAmount * Time.fixedDeltaTime);
            }


            //Rotation 
            if (forceDirection != Vector3.zero) {
                Vector3 torqueAxis = Vector3.Cross(Vector3.up, forceDirection);
                rb.AddTorque(-cameraForward * inputDirection.x * rotationTorque * Time.fixedDeltaTime);
            }



            Vector3 offset = centerOfMassOffset + new Vector3(inputDirection.x, 0, 0);
            //Recalculate center of mass 
            Vector3 worldPos = transform.position + offset;

            Vector3 localPos = transform.InverseTransformPoint(worldPos);
          
            rb.centerOfMass = localPos;
        }

        private void OnDrawGizmosSelected() {
            Rigidbody rb = GetComponent<Rigidbody>();
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.TransformPoint(rb.centerOfMass), 0.2f);
        }


        #endregion

        private void OnDestroy() {
            input.Move -= SubscribeMoveInputs;
            input.Jump -= SubscribeJumpInput;
        }
        public void HandleMovement() {
            Move(CalculateDirection(inputDirection));
        }
        public void SubscribeJumpInput(bool jumpingIsPressed) {
            switch (jumpingIsPressed) {
                case true: {
                        HandleJump();
                        break;
                    }
                case false: {
                        break;
                    }
            }

        }
        void HandleJump() {

            if (Physics.Raycast(transform.position, -Vector3.up, 1.5f)) {
                rb.AddForce(Vector3.up * JumpForce);
            }

        }
        void Move(Vector2 direction) {

        }
        Vector2 CalculateDirection(Vector2 direction) {
            if (mainCamera == null) { mainCamera = Camera.main; }
            Vector3 cameraForward = mainCamera.transform.forward.WithY(0);
            Vector3 cameraRight = mainCamera.transform.right.WithY(0);
            Vector3 dir = cameraForward * inputDirection.y + cameraRight * inputDirection.x;
            return dir;
        }
    }
}

