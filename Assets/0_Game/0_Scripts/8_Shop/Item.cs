using UnityEngine;
using System;
using System.Reflection;
public class Item : ScriptableObject, IItem, IVistor {
    public int Cost = 10;
    public string Description;
    public Sprite Label;

     public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Visit : {o.GetType().Name} ");
        }
    }
}
