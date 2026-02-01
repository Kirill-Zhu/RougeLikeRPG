using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu( menuName = "Visitor/PowerUp", fileName = "New PowerUp")]
public class PowerUp : ScriptableObject, IVistor {

    public int health;
    public int mana;
    public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Visit : {o.GetType().Name} ");
        }
    }

    public void Visit(HealthComponent HealthComponent) {
        HealthComponent.ChangeHealth(health);
    }
    public void Visit(ManaComponent manaComponent) {
        manaComponent.ChangeMana(mana);
    }
}
