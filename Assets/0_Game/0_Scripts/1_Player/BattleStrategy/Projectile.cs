using UnityEngine;

class Projectile : WeaponType {
    public float Speed;
    public float LiveDuration;

    protected override void OnTriggerEnter(Collider other) {

        if (interactionTagName == null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
                foreach (var damageType in damageTypes)
                    health.TakeDamage(damageType);
            }
        }

        if (interactionTagName != null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health) && other.CompareTag(interactionTagName)) {
                foreach (var damageType in damageTypes)
                    health.TakeDamage(damageType);

            }
        }
    }
    public class ProjectileBuilder {
        readonly GameObject prefab;
        Transform origin;
        float speed = 1;
        int damage = 1;
        DamageType[] damageTypes;
        string interactionTagName;
        float liveDuration = 5;
        public ProjectileBuilder(GameObject prefab) {
            this.prefab = prefab;
        }
        public ProjectileBuilder FromOrigin(Transform origin) { this.origin = origin; return this; }
        public ProjectileBuilder WithSpeed(float speed) { this.speed = speed; return this; }
        public ProjectileBuilder WithDamage(int damage) { this.damage = damage; return this; } //Test
        public ProjectileBuilder WithDamageTypes(params DamageType[] damageTypes) {
            this.damageTypes = damageTypes;
            return this;
        }
        public ProjectileBuilder WithInteractionTag(string tagNane) { this.interactionTagName = tagNane; return this; }
        public ProjectileBuilder WithLiveDuration(float duration) {
            liveDuration = duration; return this;
        }
        public Projectile Build() {
            var obj = Instantiate(prefab, origin.position, origin.rotation, null);

            var projectile = obj.AddComponent<Projectile>();
            projectile.enabled = true;
            projectile.Speed = speed;
            projectile.damage = damage;
            projectile.damageTypes = damageTypes;
            projectile.interactionTagName = interactionTagName;
            projectile.LiveDuration = liveDuration;
            return projectile;
        }

    }
}
