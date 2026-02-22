using UnityEngine;

public abstract class LevelUpStrategy : ScriptableObject {

    [SerializeField] protected string skillDescription;
    public virtual string GetDescription() {
        return skillDescription;
    }
}
