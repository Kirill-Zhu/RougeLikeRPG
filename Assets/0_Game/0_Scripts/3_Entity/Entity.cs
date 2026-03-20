using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(SimpleEnemyBattleContorller))]
public class Entity : MonoBehaviour, IDamagable {
    //Dorps
    public List<GameObject> DropObjectsList;  // Simple Drop On Die

    protected Rigidbody rb;  //Still dont use it directly
    protected Collider colider; // one "l" because Component has its own collider field
    public Sprite Icon;
    //Move
    public float MoveSpeed = 0;
    public float VelocityMagnitude;

    //Battle
    protected SimpleEnemyBattleContorller battleContorller;
    public GameObject WeaponPrefab;           //(Collider for mele or graphics for projectile)
    public bool InBattle;
    public float AttackRange = 1;
    public float AttackDuration = 1;
    public float DamageDelay;
    public WeaponTypeEnum WeaponType;
    public DamageType[] DamageTypes;
    public string InteractionTagName;

    [Header("For Projectiles")]
    public float ProjectilieSpeed = 0;
    public float ProjectileLiveDuration = 0;
    public int ShootShape = 0;
    public float SpreadAngle = 70;
    public int ProjectilesCountByShoot = 1;
    public bool SelfDirectedProjectile = false;
    public Transform AimTransform;

    //Health
    public HealtComponentData healthData;
    HealthComponent healthComponent;
    Action<Entity> OnDieEvent = delegate { };

    protected bool isDead = false;
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody>();
        colider = GetComponent<Collider>();
        //Health
        healthComponent = GetComponent<HealthComponent>();


        battleContorller = GetComponent<SimpleEnemyBattleContorller>();

        //Events
        healthComponent.OnDie += Die;
    }
    private void Start() {
        battleContorller.Initialize(AttackDuration, WeaponType, AttackRange, DamageDelay, DamageTypes, WeaponPrefab, InteractionTagName, ProjectilieSpeed, ProjectileLiveDuration, ShootShape, SpreadAngle, ProjectilesCountByShoot, SelfDirectedProjectile, AimTransform);
    }

    public virtual void Die() {
        isDead = true;
        OnDieEvent?.Invoke(this);
        rb.freezeRotation = true;

        gameObject.layer = LayerMask.NameToLayer("Dead");
        if (DropObjectsList != null) {
            foreach (var dropObject in DropObjectsList) {
                var obj = Instantiate(dropObject, null);
                obj.transform.position = transform.position.WithY(0);
                obj.transform.rotation = Quaternion.identity;
            }
        }

        Destroy(this.gameObject, 2);
    }
    public virtual void TakeDamage(int damage) {
        Debug.Log($"{GetType().Name} took damage {damage}");
    }
    public virtual void InitializeEvents(Action<Entity> OnDestroEvent) {
        OnDieEvent += OnDestroEvent;
    }
    private void OnDestroy() {
        OnDieEvent = null;
        healthComponent.OnDie -= Die;
    }
    //public class EnemyBuilder {
    //    GameObject prefab;

    //    Sprite icon;

    //    float moveSpeed = 0;

    //    int attackDamage = 0;
    //    int attackRange = 1;
    //    HealtComponentData healthData;


    //    public EnemyBuilder(GameObject prefab) {
    //        this.prefab = prefab;
    //    }
    //    public EnemyBuilder WithIcon(Sprite icon) { this.icon = icon; return this; }
    //    public EnemyBuilder WithMoveSpeed(float speed) { moveSpeed = speed; return this; }
    //    public EnemyBuilder WithAttackRange(int attackRange) { this.attackRange = attackRange; return this; }
    //    public EnemyBuilder WithHealthData(HealtComponentData healthData) { this.healthData = healthData; return this; }
    //    public Entity Build() {

    //        var o = Instantiate(prefab);
    //        var entity = o.AddComponent<Entity>();
    //        entity.MoveSpeed = moveSpeed;
    //        entity.AttackRange = attackRange;
    //        entity.healthData = healthData;
    //        return entity;
    //    }
    //}

    public class TypeBuilder {
        readonly GameObject prefab;
        readonly HealtComponentData healthData;

        Sprite icon;

        float moveSpeed;

        float attackRange;
        float attackDuration;
        float damageDelay;
        GameObject weaponPrefab;
        WeaponTypeEnum weaponType;
        DamageType[] damageTypes;
        //Projectile
        float projectileSpeed = 0;
        float porjectileLiveDuration = 0;
        int shootShape;
        float spreadAngle = 70;
        int projectilesCountByShoot = 1;
        bool selfDirectedProjectile = false;
        Transform aimTransform;
        string interactionTagName;
        List<GameObject> dropObjectsList;
        public TypeBuilder(GameObject prefab, HealtComponentData healthData) {
            this.prefab = prefab;
            this.healthData = healthData;
        }
        public TypeBuilder WithIcon(Sprite icon) { this.icon = icon; return this; }
        public TypeBuilder WithMoveSpeed(float moveSpeed) { this.moveSpeed = moveSpeed; return this; }
        public TypeBuilder WithAttackRange(float attackRange) { this.attackRange = attackRange; return this; }
        public TypeBuilder WithAttackDuration(float attackDuration) { this.attackDuration = attackDuration; return this; }
        public TypeBuilder WithDamageDelay(float damageDelay) { this.damageDelay = damageDelay; return this; }
        public TypeBuilder WithWeaponPrefab(GameObject weaponPrefb) { this.weaponPrefab = weaponPrefb; return this; }
        public TypeBuilder WithWeaponType(WeaponTypeEnum weaponType) { this.weaponType = weaponType; return this; }
        public TypeBuilder WithDamageTypes(DamageType[] damageTypes) { this.damageTypes = damageTypes; return this; }
        public TypeBuilder WithInteractionTag(string tagName) { this.interactionTagName = tagName; return this; }
        public TypeBuilder WithProjecitlieSPeed(float value) { this.projectileSpeed = value; return this; }
        public TypeBuilder WithProjectileLiveDuration(float value) { this.porjectileLiveDuration = value; return this; }
        public TypeBuilder ProjectilesCountByShoot(int value) { this.projectilesCountByShoot = value; return this; }
        public TypeBuilder WithShootShape(int value) { this.shootShape = value; return this; }
        public TypeBuilder WithSpreadAngle(float angle) { this.spreadAngle = angle; return this; }
        public TypeBuilder SelfDirecredProjectile(bool isSeldDirected) { this.selfDirectedProjectile = isSeldDirected; return this; }
        public TypeBuilder SetProjectileAim(Transform aimTransform) { this.aimTransform = aimTransform; return this; }
        public TypeBuilder WithDropObject(List<GameObject> dropPrefabList) { this.dropObjectsList = dropPrefabList; return this; }

        public GameObject Build(Type type) {

            var obj = Instantiate(prefab);
            obj.AddComponent(type);
            var component = obj.GetComponent(type);

            obj.TryGetComponent<HealthComponent>(out HealthComponent health);
            health.Initialize(healthData);
            //Set Icon
            FieldInfo field = component.GetType().GetField("Icon");
            if (field != null) {
                field.SetValue(component, icon);

            }
            //Set Move Speed
            field = component.GetType().GetField("MoveSpeed");
            if (field != null) {
                field.SetValue(component, moveSpeed);

            }

            //Set Attack Range
            field = component.GetType().GetField("AttackRange");
            if (field != null) {
                field.SetValue(component, attackRange);

            }
            //Set Attack Duration
            field = component.GetType().GetField("AttackDuration");
            if (field != null) {
                field.SetValue(component, attackDuration);

            }
            //Set Damage Delay
            field = component.GetType().GetField("DamageDelay");
            if (field != null) {
                field.SetValue(component, damageDelay);

            }
            //Set Weapon Type
            field = component.GetType().GetField("WeaponType");
            if (field != null) {
                field.SetValue(component, weaponType);

            }
            //Set Weapon Type
            field = component.GetType().GetField("WeaponPrefab");
            if (field != null) {
                field.SetValue(component, weaponPrefab);
                // Debug.Log($"{component.GetType().Name} has field {field.GetValue(component)}");

            }
            //Set Damage Types
            field = component.GetType().GetField("DamageTypes");
            if (field != null) {
                field.SetValue(component, damageTypes);

            }
            //Set Interaction Tag
            field = component.GetType().GetField("InteractionTagName");
            if (field != null) {
                field.SetValue(component, interactionTagName);

            }
            //Set projectileLiveDuration
            field = component.GetType().GetField("ProjectilieSpeed");
            if (field != null) {
                field.SetValue(component, projectileSpeed);

            }

            //Set projectileLiveDuration
            field = component.GetType().GetField("ProjectileLiveDuration");
            if (field != null) {
                field.SetValue(component, porjectileLiveDuration);

            }
            //Set shoot shape
            field = component.GetType().GetField("ShootShape");
            if (field != null) {
                field.SetValue(component, shootShape);

            }
            //Set SpreadAngle
            field = component.GetType().GetField("SpreadAngle");
            if (field != null) {
                field.SetValue(component, spreadAngle);
            }

            //Set ProjectilesCountByShoot
            field = component.GetType().GetField("ProjectilesCountByShoot");
            if (field != null) {
                field.SetValue(component, projectilesCountByShoot);
            }

            //Set isPorjectlele is self-Directed
            field = component.GetType().GetField("SelfDirectedProjectile");
            if (field != null) {
                field.SetValue(component, selfDirectedProjectile);
            }

            //Set aim to shoot

            field = component.GetType().GetField("AimTransform");
            if (field != null) {
                field.SetValue(component, aimTransform);

            }
            //Set Drop Object
            field = component.GetType().GetField("DropObjectsList");
            if (field != null) {
                field.SetValue(component, dropObjectsList);
            }
            return obj;
        }
    }

}
