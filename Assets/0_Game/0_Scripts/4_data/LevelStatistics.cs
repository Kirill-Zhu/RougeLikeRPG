using System;
using System.Reflection;
using UnityEngine;
using Zenject;
public class LevelStatistics : MonoBehaviour {
    [Inject] Hero hero;
    [Header("Damage recieved")]
    [SerializeField] int physicsDamageTake;
    [SerializeField] int fireDamageTake;
    [SerializeField] int coldDamageTake;

    [Header("Damage done")]
    [SerializeField] int physicsDamageDone;
    [SerializeField] int fireDamageDone;
    [SerializeField] int coldDamageDone;

    [SerializeField] LevelStatisticsData data;

    //Event Bus
    EventBinding<OnPlayerDied> onPlayerDied;
    private void Awake() {
        data.ClearData();
        hero.HealthComponent.OnTakeDamage += PlayerTookDamage;
    }

    private void OnEnable() {
        onPlayerDied = new EventBinding<OnPlayerDied>(UpdateData);
        EventBus<OnPlayerDied>.Register(onPlayerDied);

    }
    private void OnDisable() {
        EventBus<OnPlayerDied>.Deregister(onPlayerDied);
    }
    [ContextMenu("Update Data")]

    void UpdateData() {
        data.PhysicsDamageTake = physicsDamageTake;
        data.FireDamageTake = fireDamageTake;
        data.ColdDamageTake = coldDamageTake;

        data.PhysicsDamageDone = physicsDamageDone;
        data.FireDamageDone = fireDamageDone;
        data.ColdDamageDone = coldDamageDone;
    }
    public void PlayerTookDamage(object o, int value) {
        MethodInfo visitMethod = GetType().GetMethod("TookDamage", new Type[] { o.GetType(), typeof(int) });
        if (visitMethod != null && visitMethod != GetType().GetMethod("TookDamage", new Type[] { typeof(object), typeof(int) })) {
            visitMethod?.Invoke(this, new object[] { o, value });
        }
    }

    public void TookDamage(PhysicsDamageType damageType, int value) {
        physicsDamageTake += value;
    }
    public void TookDamage(FireDamageType damageType, int value) {
        fireDamageTake += value;
    }
    public void TookDamage(ColdDamageType damageType, int value) {
        coldDamageTake += value;
    }
    public void PlayerDealtDamage(object o, int value) {
        MethodInfo visitMethod = GetType().GetMethod("DealtDamage", new Type[] { o.GetType(), typeof(int) });
        if (visitMethod != null && visitMethod != GetType().GetMethod("DealtDamage", new Type[] { typeof(object), typeof(int) })) {
            visitMethod?.Invoke(this, new object[] { o, value });
        }
    }
    public void DealtDamage(PhysicsDamageType damageType, int value) {
        physicsDamageDone += value;
        Debug.Log($"Player deal {damageType.GetType().Name} +: {value} damage");
    }
    public void DealtDamage(FireDamageType damageType, int value) {
        fireDamageDone += value;
        Debug.Log($"Player deal {damageType.GetType().Name} +: {value} damage");
    }
    public void DealtDamage(ColdDamageType damageType, int value) {
        coldDamageDone += value;
        Debug.Log($"Player deal {damageType.GetType().Name} +: {value} damage");
    }
}
