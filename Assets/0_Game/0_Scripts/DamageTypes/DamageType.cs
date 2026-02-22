using System;

[System.Serializable]
public class DamageType {
    public readonly int Value;
    public DamageType(int value) {
        this.Value = value;
    }
}
[Flags]
public enum DamageTypesEnum {
    None = 0,
    Physics = 1 << 0,
    Fire = 1 << 1,
    Cold = 1 << 2,
}