using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillsStrategy : ScriptableObject, IVisitable {
    public GameObject prefab;
    public GameObject[] ParticlePrefabArray;
    protected ParticleSystem[] particleSystemArray = new ParticleSystem[0];
    protected GameObject[] particleGameObjectsArray = new GameObject[0];
    public int ManaCost = 0;
    public int PhysicsDamage;
    public int FireDamage;
    public int ColdDamage;
    public Sprite Icon;
    public Sprite BackSprite;
    public DamageTypesEnum DamageTypesEnum;
    public List<DamageType> damageTypesList = new List<DamageType>();
    protected Dictionary<Type, int> damageTypeBuffer = new Dictionary<Type, int>();
    public HealthComponent HealthComponent;                      //For Shield Strategy to add some defence
    protected Transform Origin;

    public event UnityAction<float> OnCoolDownFillAmountValue;
    public event UnityAction<int> OnManaChange;
    public float CoolDown;
    protected float coolDownTimer;
    public string AnimationName;
    public abstract int CurrentAnimationHash { get; set; }
    public float SkillDuration;

    protected void InvokeOnCoolDownCall(float value) {
        OnCoolDownFillAmountValue?.Invoke(value);
    }
    public abstract void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int, float> OnAnimation, UnityAction<int> OnManaChangEvent);
    public abstract void OnUpdate(float deltaTime);
    public abstract void Initialize(Transform origin);
    public abstract void Dispose();
    public void AddOrModifyDamageType(DamageType[] damageType) {
        foreach (var damage in damageType) {
            SetOrAddDamageTypeWithValues(damage);
        }

        damageTypesList.Clear();

        if (damageTypeBuffer.TryGetValue(typeof(PhysicsDamageType), out var physics)) {
            Debug.Log($"Physics Damage in buffer is {physics}");
            damageTypesList.Add(new PhysicsDamageType(physics)); 
        
        }
        if (damageTypeBuffer.TryGetValue(typeof(FireDamageType), out var fire)) damageTypesList.Add(new FireDamageType(fire));
        if (damageTypeBuffer.TryGetValue(typeof(ColdDamageType), out var cold)) damageTypesList.Add(new ColdDamageType(cold));
    }
    protected void SetOrAddDamageTypeWithValues<T>(T inType) where T : DamageType {


        if (damageTypeBuffer.TryGetValue(inType.GetType(), out var outDamage)) {
            Debug.Log($"Modify damage type {inType}");
            damageTypeBuffer[inType.GetType()] = outDamage + inType.Value;
            Debug.Log($"now damage buffer damage is {damageTypeBuffer[inType.GetType()]}");

        }
        else {
            Debug.Log($"Add damage type {inType}");
            damageTypeBuffer.Add(inType.GetType(), inType.Value);
        }

        //if (outType == null) {
        //    //Add new Type
        //    Debug.Log($"Add damage type {inType}");
        //    damageTypeBuffer.Add(inType.GetType(), new DamageType(inType.Value));
        //} else {
        //    //Modify type
        //    Debug.Log($"Modify damage type {inType}");
        //    int damage = damageTypeBuffer[inType.GetType()].Value + inType.Value;
        //    damageTypeBuffer[inType.GetType()] = new DamageType(damage);

        //}
    }
    protected virtual DamageType[] GetStartDamageTypes() => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage) },
        DamageTypesEnum.Fire => new DamageType[] { new FireDamageType(FireDamage) },
        DamageTypesEnum.Cold => new DamageType[] { new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };

    public abstract void UpdateValues();
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }

    private void OnDestroy() {
        OnCoolDownFillAmountValue = null;
    }
}

