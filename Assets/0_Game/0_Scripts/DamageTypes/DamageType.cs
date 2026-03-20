using System;
using UnityEngine;

[System.Serializable]
public class DamageType {
    public readonly int Value;

    public DamageType(int value) {
        this.Value = value;
    }

    public static implicit operator Color(DamageType damageType) {
        if (damageType.GetType() == typeof(PhysicsDamageType)) {
            return Color.white;
        }
        if (damageType.GetType() == typeof(FireDamageType)) {
            return Color.yellow;
        }
        if (damageType.GetType() == typeof(ColdDamageType)) {
            return Color.cyan;
        }
        return Color.white;
    }
}
[Flags]
public enum DamageTypesEnum {
    None = 0,
    Physics = 1 << 0,
    Fire = 1 << 1,
    Cold = 1 << 2,
}
public struct DamageTypesStruct {
    public readonly int PhysicsDamage;
    public readonly int FireDamage;
    public readonly int ColdDamage;
}