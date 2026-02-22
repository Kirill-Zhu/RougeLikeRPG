using System;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(SimpleEnemyBattleContorller))]
public class Entity : MonoBehaviour, IDamagable {
    //Dorps
    public GameObject DropObject;  // Simple Drop On Die

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
    //Health
    public HealtComponentData healthData;
    HealthComponent healthComponent;
    Action<Entity> OnDieEvent = delegate { };
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody>();
        colider = GetComponent<Collider>();

        healthComponent = GetComponent<HealthComponent>();
        battleContorller = GetComponent<SimpleEnemyBattleContorller>();

        //Events
        healthComponent.OnDie += Die;
    }
    private void Start() {


        battleContorller.Initialize(AttackDuration, WeaponType, AttackRange, DamageDelay, DamageTypes, WeaponPrefab, InteractionTagName);
    }

    public virtual void Die() {
        OnDieEvent?.Invoke(this); 
        rb.freezeRotation = true;

        gameObject.layer = LayerMask.NameToLayer("Dead");
        if(DropObject != null) {
            var obj = Instantiate(DropObject, null);
            obj.transform.position = transform.position.WithY(0);
            obj.transform.rotation = Quaternion.identity;   
        }

        Destroy(this.gameObject,2);
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
        GameObject prefab;

        Sprite icon;

        float moveSpeed;

        float attackRange;
        float attackDuration;
        float damageDelay;
        GameObject weaponPrefab;
        WeaponTypeEnum weaponType;
        DamageType[] damageTypes;
        string interactionTagName;
        GameObject dropObject;
        HealtComponentData healthData;
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
        public TypeBuilder WithInteractionTag(string tagName) { this.interactionTagName =  tagName; return this; }
        public TypeBuilder WithDropObject(GameObject dropPrefab) { this.dropObject = dropPrefab; return this; }
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

            //Set Drop Object
            field = component.GetType().GetField("DropObject");
            if (field != null) {
                field.SetValue(component, dropObject);

            }
            return obj;
        }
    }

}
