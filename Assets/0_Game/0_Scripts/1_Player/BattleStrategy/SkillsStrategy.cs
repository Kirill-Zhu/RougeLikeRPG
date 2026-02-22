using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillsStrategy : ScriptableObject, IVisitable {
    public GameObject prefab;
    public int PhysicsDamage;
    public int FireDamage;
    public int ColdDamage;
    public Sprite Icon;
    public Sprite BackSprite;
    public DamageTypesEnum DamageTypesEnum;
    public DamageType[] damageTypesArray;
    public HealthComponent HealthComponent;                      //For Shield Strategy to add some defence
    protected Transform Origin;

    public event UnityAction<float> OnCoolDownFillAmountValue;
    public float CoolDown;
    protected float coolDownTimer;
    public string AnimationName;
    public abstract int CurrentAnimationHash { get; set; }
    public float SkillDuration;

    protected void InvokeOnCoolDownCall(float value) {
        OnCoolDownFillAmountValue?.Invoke(value);
    }
    public abstract void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int,float> OnAnimation);
    public abstract void OnUpdate(float deltaTime);
    public abstract void Initialize(Transform origin);

    protected virtual DamageType[] GetDamageTypes() => DamageTypesEnum switch {
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

