using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponType : WeaponType {
    public float LiveDuration;
    float timer;
    HashSet<HealthComponent> victimsList = new HashSet<HealthComponent>();
    protected override void OnTriggerEnter(Collider other) {
        if (interactionTagName == null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
                foreach (var damageType in damageTypes)
                    victimsList.Add(health);
            }
        }

        if (interactionTagName != null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health) && other.CompareTag(interactionTagName)) {
                foreach (var damageType in damageTypes)
                    victimsList.Add(health);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (interactionTagName == null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health)) {
                foreach (var damageType in damageTypes)
                    victimsList.Remove(health);
            }
        }

        if (interactionTagName != null) {
            if (other.TryGetComponent<HealthComponent>(out HealthComponent health) && other.CompareTag(interactionTagName)) {
                foreach (var damageType in damageTypes)
                    victimsList.Remove(health);
            }
        }
    }


    private void Update() {
        LiveDuration -= Time.deltaTime;
        if (LiveDuration < 0) Destroy(this.gameObject);
        if (timer < 1) {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;

        foreach (var victim in victimsList) {
            foreach(var damageType in damageTypes)
                victim.TakeDamage(damageType);

           // Debug.Log($"{victim.name} tooke damae {damageTypes[0].GetType()} {damageTypes[0].Value} ");
        }
    }
    public class AreaWeaponTypeBuilder {
        readonly GameObject prefab;
        Transform origin;

        int damagePerSec = 1;
        DamageType[] damageTypes;
        string interactionTagName;
        float liveDuration = 5;
        public AreaWeaponTypeBuilder(GameObject prefab) {
            this.prefab = prefab;
        }
        public AreaWeaponTypeBuilder FromOrigin(Transform origin) { this.origin = origin; return this; }

        public AreaWeaponTypeBuilder WithDamageTypes(params DamageType[] damageTypes) {
            this.damageTypes = damageTypes;
            return this;
        }
        public AreaWeaponTypeBuilder WithInteractionTag(string tagNane) { this.interactionTagName = tagNane; return this; }
        public AreaWeaponTypeBuilder WithLiveDuration(float duration) {
            liveDuration = duration; return this;
        }
        public AreaWeaponType Build() {
            var obj = Instantiate(prefab, origin.position, origin.rotation, null);

            var areaWepon = obj.AddComponent<AreaWeaponType>();
            areaWepon.enabled = true;
            areaWepon.damageTypes = damageTypes;
            areaWepon.interactionTagName = interactionTagName;
            areaWepon.LiveDuration = liveDuration;
            return areaWepon;
        }

    }
}