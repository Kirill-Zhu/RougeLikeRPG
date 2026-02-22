using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Visitor/PowerUp", fileName = "New PowerUp")]
public abstract class PowerUp : ScriptableObject, IVistor {

    public GameObject ModelPrefab;
    public GameObject particle;

    public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Visit : {o.GetType().Name} ");
        }
    }
  
}
