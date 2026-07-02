using UnityEngine;

public class WheelController : MonoBehaviour
{
  
    
        [Header("PID Настройки (Ось X)")]
        public float pGain = 100f;
        public float dGain = 10f;
        public float iGain = 0.5f;

        [Header("Настройки Управления")]
        [Tooltip("Максимальный угол наклона/поворота при нажатии клавиш.")]
        public float maxSteerAngle = 25f;
        [Tooltip("Скорость перехода в наклон.")]
        public float steerSpeed = 5f;

        private Rigidbody rb;
        private float integralError = 0f;
        private float targetXAngle = 0f;

        void Start() {
            rb = GetComponent<Rigidbody>();
            // Для балансирующих роботов/колес полезно слегка занизить центр масс
            rb.centerOfMass = new Vector3(0, -0.2f, 0);
        }

        void FixedUpdate() {
            HandleInput();
            StabilizeAndSteer();
        }

        void HandleInput() {
            // Считываем ввод (A/D или Стрелки влево/вправо)
            // Возвращает значение от -1 (влево) до 1 (вправо)
            float moveInput = Input.GetAxis("Horizontal");

            // Меняем целевой угол в зависимости от ввода
            // Если крутим влево — наклоняем в одну сторону, вправо — в другую
            float desiredAngle = moveInput * maxSteerAngle;

            // Плавный переход к новому углу, чтобы не было резких рывков
            targetXAngle = Mathf.Lerp(targetXAngle, desiredAngle, Time.fixedDeltaTime * steerSpeed);
        }

        void StabilizeAndSteer() {
            // 1. Считаем текущий наклон по оси X
            float currentXAngle = transform.localEulerAngles.x;
            if (currentXAngle > 180f) currentXAngle -= 360f;

            // Ошибка — это разница между ТЕКУЩИМ углом и ЦЕЛЕВЫМ (заданным управлением)
            float error = targetXAngle - currentXAngle;

            // 2. Интегральная составляющая
            integralError += error * Time.fixedDeltaTime;
            // Ограничиваем во избежание «эффекта пружины» при долгом заносе
            integralError = Mathf.Clamp(integralError, -20f, 20f);

            // 3. Дифференциальная составляющая (угловая скорость по локальной оси X)
            float currentAngularVelocityX = transform.InverseTransformDirection(rb.angularVelocity).x;

            // 4. Формула PID для оси X
            float torqueOutput = (error * pGain) + (integralError * iGain) - (currentAngularVelocityX * dGain);

            // 5. Применяем локальный крутящий момент по оси X через AddRelativeTorque
            rb.AddRelativeTorque(Vector3.right * torqueOutput, ForceMode.Acceleration);
        }
   
}
