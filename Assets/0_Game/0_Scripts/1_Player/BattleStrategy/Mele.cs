using UnityEngine;


class Mele : WeaponType {
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

    public class MeleBuilder {
        DamageType[] damageTypes;
        int damage = 1;
        string interactionTagName;
        GameObject prefab;
        Transform origin;
        Vector3 offset;

        public MeleBuilder(GameObject prefab) { this.prefab = prefab; }
        public MeleBuilder FromOrigin(Transform origin) { this.origin = origin; return this; }
        public MeleBuilder WithOffset(Vector3 offset) { this.offset = offset; return this; }
        public MeleBuilder WithDamageTypes(params DamageType[] damageTypes) {
            this.damageTypes = damageTypes;
            return this;
        }
        public MeleBuilder WithInteractionTag(string tagName) { interactionTagName = tagName; return this; }

        public Mele Build() {

            offset = origin.transform.TransformDirection(offset);
            var obj = Instantiate(prefab, origin.position + offset, origin.rotation, origin);
            var mele = obj.AddComponent<Mele>();
            mele.enabled = true;
            mele.damage = damage;
            mele.damageTypes = damageTypes;
            mele.interactionTagName = interactionTagName;
            return mele;
        }

    }
}