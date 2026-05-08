using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponType : WeaponType {
    public Transform Origin;
    public bool DestroyAfterUse = true;
    public float LiveDuration;
    public bool FollowCaster;
    float liveTimer;
    float tickTimer;
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

    private void OnEnable() {
        liveTimer = LiveDuration;
        if (Origin != null)
            transform.position = Origin.position;
    }
    private void OnDisable() {
        victimsList.Clear();
    }

    private void Update() {
        liveTimer -= Time.deltaTime;

        if (liveTimer < 0) {
            gameObject.SetActive(false);

        } else if (liveTimer < 0 && DestroyAfterUse) {
            Destroy(this.gameObject);
        }
        //Tick timer
        if (tickTimer < 1) {
            tickTimer += Time.deltaTime;
            return;
        }
        tickTimer = 0;

        foreach (var victim in victimsList) {
            foreach (var damageType in damageTypes)
                victim.TakeDamage(damageType);
            // Debug.Log($"{victim.name} tooke damae {damageTypes[0].GetType()} {damageTypes[0].Value} ");
        }

    }
    public class Builder {
        readonly GameObject prefab;
        Transform origin;
        DamageType[] damageTypes;
        string interactionTagName;
        float liveDuration = 5;
        bool followHero = true;
        bool destroyAfterUse = false;
        public Builder(GameObject prefab) {
            this.prefab = prefab;
        }
        public Builder FromOrigin(Transform origin) { this.origin = origin; return this; }

        public Builder WithDamageTypes(params DamageType[] damageTypes) {
            this.damageTypes = damageTypes;
            return this;
        }
        public Builder WithInteractionTag(string tagNane) { this.interactionTagName = tagNane; return this; }
        public Builder WithLiveDuration(float duration) {
            liveDuration = duration; return this;
        }
        public Builder FollowCaster(bool value) { this.followHero = value; return this; }
        public Builder DestroyAfterUse(bool destroy) {
            this.destroyAfterUse = destroy; return this;
        }
        public AreaWeaponType Build() {
            Transform parent;
            var obj = Instantiate(prefab, origin.position, origin.rotation, parent = followHero ? origin : null);

            var areaWepon = obj.AddComponent<AreaWeaponType>();
            areaWepon.enabled = true;
            areaWepon.Origin = origin;
            areaWepon.damageTypes = damageTypes;
            areaWepon.interactionTagName = interactionTagName;
            areaWepon.LiveDuration = liveDuration;
            areaWepon.FollowCaster = followHero;
            areaWepon.DestroyAfterUse = destroyAfterUse;
            return areaWepon;
        }

    }
}