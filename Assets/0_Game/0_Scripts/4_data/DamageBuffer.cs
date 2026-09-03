using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DamageBuffer : MonoBehaviour {
    private struct DamageCall {
        public MethodInfo Method;
        public object DamageObject;
        public HealthComponent TargetHealth;
    }

    private Queue<DamageCall> damageCalls = new Queue<DamageCall>();

    [SerializeField] private int maxCallsPerFrame = 20;

    
    private object[] sharedArgsBuffer = new object[1];

    public void RegisterDamage(MethodInfo visitMethod, object o, HealthComponent healthComponent) {
        if (visitMethod == null || healthComponent == null) return;

        damageCalls.Enqueue(new DamageCall {
            Method = visitMethod,
            DamageObject = o,
            TargetHealth = healthComponent
        });
    }

    private void Update() {
        int processedCount = 0;

        while (damageCalls.Count > 0 && processedCount < maxCallsPerFrame) {
            DamageCall call = damageCalls.Dequeue();

            if (call.TargetHealth != null && call.Method != null) {
                // ИСПРАВЛЕНО: Записываем объект в нулевую ячейку массива
                sharedArgsBuffer[0] = call.DamageObject;

                call.Method.Invoke(call.TargetHealth, sharedArgsBuffer);

                processedCount++;
            }
        }

        // ИСПРАВЛЕНО: Очищаем элемент внутри массива, а не сам массив
        sharedArgsBuffer[0] = null;
    }
}
