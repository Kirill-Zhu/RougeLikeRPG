using UnityEngine;


namespace BossEntity {
    public class MoveController : MonoBehaviour {

        [SerializeField] float moveSpeed = 1;
        [SerializeField] float stopDistancce = 3;
        [SerializeField] float rotationSpeed = 8;
        [SerializeField] float nearHeroDistance = 15;
        float speedModifier = 1;
        Transform target;
        public float Velocity => speedModifier;
        public float DistanceToHero => Vector3.Distance(transform.position, target.position);
        public void Initialize(Transform target) {
            this.target = target;
        }
        public void OnUpdate() {
            MoveToTarget(target.position);
        }

        void MoveToTarget(Vector3 target) {
            if (DistanceToHero < stopDistancce) { speedModifier = 0; return; }
            if (DistanceToHero < nearHeroDistance) speedModifier = 0.5f;
            else speedModifier = 1;

            Vector3 direction = target - transform.position;
            transform.Translate(direction.normalized * moveSpeed * speedModifier * Time.deltaTime, Space.World);

            if (direction != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(direction.WithY(0));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}