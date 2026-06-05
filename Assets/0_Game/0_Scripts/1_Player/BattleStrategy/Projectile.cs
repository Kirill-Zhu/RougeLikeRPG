using UnityEngine;

public class Projectile : WeaponType {
    public float Speed;
    public float LiveDuration;
    public bool SelfDirected = false;

    protected override void OnTriggerEnter(Collider other) {

        if (interactionTagName == null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
               DoDamage(health);
            }
        }

        if (interactionTagName != null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health) && other.CompareTag(interactionTagName)) {
                DoDamage(health);
            }
        }
    }
    public class ProjectileBuilder {
        readonly GameObject prefab;
        Transform origin;
        float speed = 1;
        DamageType[] damageTypes;
        string interactionTagName;
        float liveDuration = 5;
        bool seifDirected = false;
        public ProjectileBuilder(GameObject prefab) {
            this.prefab = prefab;
        }
        public ProjectileBuilder FromOrigin(Transform origin) { this.origin = origin; return this; }
        public ProjectileBuilder WithSpeed(float speed) { this.speed = speed; return this; }
        public ProjectileBuilder WithDamageTypes(params DamageType[] damageTypes) {
            this.damageTypes = damageTypes;
            return this;
        }
        public ProjectileBuilder WithInteractionTag(string tagNane) { this.interactionTagName = tagNane; return this; }
        public ProjectileBuilder WithLiveDuration(float duration) {
            liveDuration = duration; return this;
        }
        public ProjectileBuilder SelfDirected(bool isSetlfDirected) { this.seifDirected = isSetlfDirected; return this; }
        public Projectile Build() {
            var obj = Instantiate(prefab, origin.position, origin.rotation, null);

            var projectile = obj.AddComponent<Projectile>();
            projectile.enabled = true;
            projectile.Speed = speed;
            projectile.baseDamage = damageTypes;
            projectile.interactionTagName = interactionTagName;
            projectile.LiveDuration = liveDuration;
            projectile.SelfDirected = seifDirected;
            return projectile;
        }

    }
}
