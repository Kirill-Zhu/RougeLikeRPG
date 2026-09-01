using UnityEngine;
using System;
using System.Reflection;
public class EntityHealthComponent : MonoBehaviour, IVisitable {
    
    HealtComponentData healthData;
    DamageBuffer damageBuffer;

    public void Accept(IVistor visitor) {
       visitor.Visit(this);
    }

    public void Initialize(DamageBuffer damageBuffer, HealtComponentData healthData) {
        this.damageBuffer = damageBuffer;   
        this.healthData = healthData;
    }

    public void TakeDamage(object o) {
        //if (Invincible) return;


        MethodInfo visitMethod = GetType().GetMethod("EarnDamageByType", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("EarnDamageByType", new Type[] { typeof(object) })) {
            visitMethod?.Invoke(this, new object[] { o });
            // Debug.Log($"EarnDamageByType : {o.GetType().Name} ");
        }
    }


}
