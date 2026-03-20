using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AutoSkillStrategy : ScriptableObject {

    [SerializeField] protected GameObject prefab;
    //UI
    public Sprite icon;
    public bool parentToHero = true;
    //VFX
    public GameObject[] ParticlePrefabArray;
    protected ParticleSystem[] particleSystemArray = new ParticleSystem[0];
    protected GameObject[] particleGameObjectsArray = new GameObject[0];

    //Damage
    [SerializeField] DamageTypesEnum DamageTypesEnum;
    public int PhysicsDamage;
    public int FireDamage;
    public int ColdDamage;
    public float PushPower;
    protected List<DamageType> damageTypesList;
    protected WeaponType weaponType;
    [SerializeField] protected float coolDown = 1;
    public const string interactionTagName = "Enemy";
    protected float coolDownTimer = 0;
    protected Transform Origin;

    //Events
    public UnityAction<float> OnCoolDownFillAmountValue;
    public abstract void Initialize(Transform origin);
    public abstract void OnUpdate(float deltaTime);
    protected void InvokeOnCoolDownCall(float value) {
        OnCoolDownFillAmountValue?.Invoke(value);
    }
    protected virtual DamageType[] GetStartDamageTypes() => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage) },
        DamageTypesEnum.Fire => new DamageType[] { new FireDamageType(FireDamage) },
        DamageTypesEnum.Cold => new DamageType[] { new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };
    protected abstract void UseSKill();

    private void OnDestroy() {
        OnCoolDownFillAmountValue = null;   
    }
}
