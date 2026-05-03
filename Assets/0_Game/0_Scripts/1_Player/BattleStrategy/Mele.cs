using UnityEngine;


class Mele : WeaponType {
    protected override void OnTriggerEnter(Collider other) {

        if (interactionTagName == null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
                //Set total damage
                for (int i = 0; i < damageTypes.Length; i++) {
                    totalDamage[i].ResetToZero();
                    totalDamage[i].AddDamage(damageTypes[i].Value + bonusDamageTypes[i].Value);
                }
                //Do total Damage
                foreach (var damage in totalDamage) {
                    health.TakeDamage(damage);
                }
            }
        }

        if (interactionTagName != null && other.CompareTag(interactionTagName)) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
                
                //Set total damage
                for (int i = 0; i < damageTypes.Length; i++) {
                    totalDamage[i].ResetToZero();
                    totalDamage[i].AddDamage(damageTypes[i].Value + bonusDamageTypes[i].Value);
                }
                
                //Do TOtal damage
                foreach (var damage in totalDamage) {
                    health.TakeDamage(damage);
                }
            }
            //if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
            //    foreach (var damageType in damageTypes)
            //        health.TakeDamage(damageType);

            //    //Bonus damage
            //    foreach (var bonusDamage in bonusDamageTypes) {
            //        health.TakeDamage(bonusDamage);
            //        Debug.Log($"Bonus damage is {bonusDamage.Value}");
            //    }
            //}
            if (pushPower != 0 && other.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = (rb.transform.position - transform.position) * pushPower;
        }

    }

    public class MeleBuilder {
        DamageType[] damageTypes;
        float pushPower;
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
        public MeleBuilder WithPushPower(float value) { this.pushPower = value; return this; }
        public MeleBuilder WithInteractionTag(string tagName) { interactionTagName = tagName; return this; }

        public Mele Build() {

            offset = origin.transform.TransformDirection(offset);
            var obj = Instantiate(prefab, origin.position + offset, origin.rotation, origin);
            var mele = obj.AddComponent<Mele>();
            mele.enabled = true;
            mele.damageTypes = damageTypes;
            mele.pushPower = pushPower;
            mele.interactionTagName = interactionTagName;
            // Debug.Log($"new weapom has physics Damage {damageTypes[0].Value}");
            return mele;
        }

    }
}