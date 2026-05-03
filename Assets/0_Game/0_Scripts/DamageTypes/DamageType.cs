using System;
using UnityEngine;

[System.Serializable]
public class DamageType {
    public int Value => value;
    int value;
    public DamageType(int value) {
        this.value = value;
    }
    public void AddDamage(int value) => this.value += value;
    public void RemoveBonus(int value) => this.value -= value;
    public void ResetToZero() {
        value = 0;
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
    public static implicit operator float(DamageType damageType) {
        if (damageType.GetType() == typeof(PhysicsDamageType)) {
            return 2;
        }
        if (damageType.GetType() == typeof(FireDamageType)) {
            return 3;
        }
        if (damageType.GetType() == typeof(ColdDamageType)) {
            return 4;
        }
        return 0;
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