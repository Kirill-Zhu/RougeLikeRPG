using UnityEngine;

public abstract class SkillsStrategy : ScriptableObject {
    public abstract float CoolDownTimer { get; set; }
    public abstract void UseSkill(Transform origin, out float skillDuration);
    public abstract void OnUpdate(float deltaTime);
}

