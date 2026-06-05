using FMODUnity;
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
    [Header("Level Up")]
   [SerializeField] AutoSkillStrategy nextLevelStrategy;

    //Level UP
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
    [Header("Sound")]
    [SerializeField] protected EventReference OnCastSound;
    //Events
    public UnityAction<float> OnCoolDownFillAmountValue;
    public abstract void Initialize(Transform origin);
    public abstract void OnUpdate(float deltaTime);
    protected void InvokeOnCoolDownCall(float value) {
        OnCoolDownFillAmountValue?.Invoke(value);
    }
    protected virtual DamageType[] GetStartDamageTypes() => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(0), new ColdDamageType(0) },
        DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(0), new FireDamageType(FireDamage), new ColdDamageType(0) },
        DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(0) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(0), new ColdDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };
    protected abstract void UseSKill();
    protected abstract void PlayOnCastSound();
    public virtual T UpgrageSkill<T>(T strategy) where T : AutoSkillStrategy {
        return (T)nextLevelStrategy;
    }

    public abstract void Dispose();
    private void OnDestroy() {
        OnCoolDownFillAmountValue = null;
    }
}
