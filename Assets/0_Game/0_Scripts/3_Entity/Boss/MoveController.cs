using UnityEngine;


namespace BossEntity {
    public class MoveController : MonoBehaviour {

        [SerializeField] float moveSpeed = 1;
        [SerializeField] float stopDistancce = 3;
        [SerializeField] float rotationSpeed = 8;
        Transform target;
        public float DistanceToHero => Vector3.Distance(transform.position, target.position);
        public void Initialize(Transform target) {
            this.target = target;
        }
        public void OnUpdate() {
            MoveToTarget(target.position);
        }

        void MoveToTarget(Vector3 target) {
            if (DistanceToHero < stopDistancce) { return; }

            transform.Translate((target - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
            Vector3 direction = target - transform.position;

            if (direction != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(direction.WithY(0));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}