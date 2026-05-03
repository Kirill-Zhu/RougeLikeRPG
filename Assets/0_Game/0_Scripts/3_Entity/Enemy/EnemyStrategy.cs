using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Enemy/NewEnemyType", fileName = "NewEnemyType")]
public class EnemyStrategy : ScriptableObject {
    public string TypeOfEnemy;
    public GameObject prefab;
    public Sprite Icon;
    public float MoveSpeed;
    public float AttackRange;
    public float AttackDuration;
    public float DamageDelay;

    [Header("For Projectiles")]
    public float ProjectileSpeed = 1;
    public float ProjectilieLiveDureation = 1;
    public ShootShape ShootShape = ShootShape.Forward;
    public float SpreadAngle = 70;
    public int ProjecitlesCountByShoot = 1;
    public bool SelfDirecrtedProjectile = false;
    public GameObject WeaponPrefab;
    public WeaponTypeEnum WeaponType;
    public List<GameObject> DropPfreabList;
    [SerializeField] DamageTypesEnum damageTypesEnum;
    [SerializeField] int physicsDamage;
    [SerializeField] int fireDamage;
    [SerializeField] int coldDamage;
    public HealtComponentData HealtData;

    [Header("Sound")]
    public EventReference OnAttack;
    public EventReference OnDie;
    //Here  I can manipulate with types like Cold and Fire may be Steam or something else
    public DamageType[] GetDamageTypes() => damageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(physicsDamage) },
        DamageTypesEnum.Fire => new DamageType[] { new FireDamageType(fireDamage) },
        DamageTypesEnum.Cold => new DamageType[] { new ColdDamageType(coldDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(physicsDamage), new FireDamageType(fireDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(physicsDamage), new FireDamageType(coldDamage) },

        _ => new DamageType[] { new PhysicsDamageType(physicsDamage) },
    };
}
