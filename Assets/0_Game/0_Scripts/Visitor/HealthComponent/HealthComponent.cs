using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
public class HealthComponent : MonoBehaviour, IVisitable, IUpgradable {

    [Header("Initialize Properties")]
    [SerializeField] PopUpDamageValuesHandler popUpStrategy;
    PopUpDamageValuesHandler popUpHandler;
    int startMaxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField] int maxHealth = 100;
    public int Health => health;
    [SerializeField] int health = 100;
    [SerializeField] int physicsDefence = 2;
    [SerializeField] int fireDefence = 1;
    [SerializeField] int coldDefence = 1;
    public event UnityAction<int> OnGetCurrentHealth;   // For health globes
    public event UnityAction<DamageType, int> OnTakeDamage;
    public event Action<DamageType> OnTakeDamageBus;
    public event UnityAction<DamageType> OnBlockDamage;
    public event UnityAction OnDie;
    CancellationTokenSource cts; //Use on destroy to kill dotween animations in popUpStrategy
    CancellationToken token;

    //Upgrades
    List<Item> itemsList = new List<Item>();
   
    public void Initialize(HealtComponentData healtData) {
        //CTS
        cts = new CancellationTokenSource();
        token = cts.Token;

        popUpStrategy = healtData.popUpDamageValuesHandler;
        maxHealth = healtData.MaxHealth;
        startMaxHealth = maxHealth;
        health = maxHealth;
        physicsDefence = healtData.PhysicsDefence;
        fireDefence = healtData.FireDefence;
        coldDefence = healtData.ColdDefence;

        if (popUpStrategy != null && popUpHandler == null) {
            popUpHandler = Instantiate(popUpStrategy);
            popUpHandler.Initialize(transform, token);
            OnTakeDamage += popUpHandler.PupUpDamage;
            OnBlockDamage += popUpHandler.PopUpBlock;
        }
    }
    private void OnDestroy() {
        cts.Cancel();
        OnTakeDamage -= popUpHandler.PupUpDamage;
        OnBlockDamage -= popUpHandler.PopUpBlock;
        Destroy(popUpHandler);
    }
    public void ChangeHealth(int value) {
        health += value;
        if (health > maxHealth)
            health = maxHealth;

        // OnHealthChange?.Invoke(value);
        OnGetCurrentHealth?.Invoke(health);
        if (health <= 0) OnDie?.Invoke();
    }

    //Visitor
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }


    #region Take Damage

    //public void TakeDamage(DamageTypesStruct damageTypes) {
    //    if (damageTypes.PhysicsDamage <= physicsDefence) {
    //        OnBlockDamage?.Invoke(new PhysicsDamageType(damageTypes.PhysicsDamage));
    //        return;
    //    }
    //    if (damageTypes.FireDamage <= fireDefence) {
    //        OnBlockDamage?.Invoke(new FireDamageType(damageTypes.FireDamage));
    //        return;
    //    }
    //    if(damageTypes.ColdDamage <= coldDefence) {
    //        OnBlockDamage?.Invoke(new FireDamageType(damageTypes.ColdDamage));
    //        return;
    //    }
    //}
    public void TakeDamage(object o) {
        //if (Invincible) return;

        MethodInfo visitMethod = GetType().GetMethod("EarnDamageByType", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("EarnDamageByType", new Type[] { typeof(object) })) {
            visitMethod?.Invoke(this, new object[] { o });
            // Debug.Log($"EarnDamageByType : {o.GetType().Name} ");
        }
    }
    public void EarnDamageByType(PhysicsDamageType damageType) {

        if (damageType.Value <= physicsDefence) {
            OnBlockDamage?.Invoke(damageType);
            return;
        }
        var damage = damageType.Value - physicsDefence;
        ChangeHealth(-damage);
        OnTakeDamage?.Invoke(damageType, damage);
        OnTakeDamageBus?.Invoke(damageType);
        //Debug.Log($"{this.gameObject.name} get {damageType.GetType()} damage {damageType.Value-physicsDefence} ");
    }
    public void EarnDamageByType(FireDamageType damageType) {
        if (damageType.Value <= fireDefence) {
            OnBlockDamage?.Invoke(damageType);
            return;
        }

        var damage = damageType.Value - fireDefence;
        ChangeHealth(-damage);
        OnTakeDamage?.Invoke(damageType, damage);
        OnTakeDamageBus?.Invoke(damageType);
        // Debug.Log($"{this.gameObject.name} get {damageType.GetType()} damage {damageType.Value-fireDefence}");
    }
    public void EarnDamageByType(ColdDamageType damageType) {
        if (damageType.Value <= coldDefence) {
            OnBlockDamage?.Invoke(damageType);
            return;
        }

        var damage = damageType.Value - coldDefence;
        ChangeHealth(-damage);
        OnTakeDamage?.Invoke(damageType, damage);
        OnTakeDamageBus?.Invoke(damageType);
        // Debug.Log($"{this.gameObject.name} get {damageType.GetType()} damage {damageType.Value-coldDefence} ");
    }

    #endregion

    #region Shield Strategty
    //Set Shield Protection
    public void SetShieldProtection(object o) {
        MethodInfo visitMethod = GetType().GetMethod("SetShieldProtection", new Type[] { o.GetType() });
        Debug.Log($"Set Shield From : {o.GetType().Name} ");
        if (visitMethod != null && visitMethod != GetType().GetMethod("SetShieldProtection", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            // Debug.Log($"Set Shield From : {o.GetType().Name} ");
        }

    }
    public void SetShieldProtection(PhysicsDamageType type) {
        physicsDefence += type.Value;
        //Debug.Log($"Set Shield protecion {type.GetType().Name}");
    }
    public void SetShieldProtection(FireDamageType type) {
        fireDefence += type.Value;
        //Debug.Log($"Set Shield protecion {type.GetType().Name}");
    }
    public void SetShieldProtection(ColdDamageType type) {
        coldDefence += type.Value;
        //Debug.Log($"Set Shield protecion {type.GetType().Name}");
    }

    //Remove Protection

    public void RemoveShieldProtection(object o) {
        MethodInfo visitMethod = GetType().GetMethod("RemoveShieldProtection", new Type[] { o.GetType() });
        Debug.Log($"Set Shield From : {o.GetType().Name} ");
        if (visitMethod != null && visitMethod != GetType().GetMethod("RemoveShieldProtection", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Remove Shield From : {o.GetType().Name} ");
        }

    }

    public void RemoveShieldProtection(PhysicsDamageType type) {
        physicsDefence -= type.Value;
    }
    public void RemoveShieldProtection(FireDamageType type) {
        fireDefence -= type.Value;
    }
    public void RemoveShieldProtection(ColdDamageType type) {
        coldDefence -= type.Value;
    }
    #endregion
    #region UPGRADE STRATEGY
    public void AddItem(Item item) {
        if (item is HeatlhItem)
            itemsList.Add(item as HeatlhItem);
    }

    public void ClearItems() {
        itemsList.Clear();
    }

    public void RefreshItemUpgrades() {
        maxHealth = startMaxHealth;
        foreach (HeatlhItem item in itemsList) {
            maxHealth += item.AddMaxHealth;
        }
        health = maxHealth;
    }
    #endregion
}