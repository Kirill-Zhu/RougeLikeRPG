using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class SimpleEnemyBattleContorller : MonoBehaviour {
    //Initialize Values
    float attackDuration;
    float damageDelay;
    WeaponTypeEnum weaponType;
    string interacitonTag;
    float attackRange;
    public List<WeaponType> weaponPoolList = new List<WeaponType>();

    public event Action OnAttack;
    event Action OnExitBattleState;
    //-------------------

    float skillDurationCooldown;
    public event Action<int> OnAnimationStart;
    public event Action<float> OnChangeDurationSkill;
   

    //Animations
    int animationHash = Animator.StringToHash("Battle");

    //UniTask
    CancellationTokenSource cts;

    public void Initialize(float attackDuration, WeaponTypeEnum weaponType, float attackRange, float damageDelay, DamageType[] damageTypes, GameObject prefab, string interactionTag = null, float projectileSpeed = 2, float ProjectileliveDuration = 2) {
        this.attackDuration = attackDuration;
        this.weaponType = weaponType;
        this.attackRange = attackRange;
        this.damageDelay = damageDelay;
        this.interacitonTag = interactionTag;
        //Mele
        if (weaponType == WeaponTypeEnum.mele) {
            var mele = new Mele.MeleBuilder(prefab)
                .FromOrigin(transform)
                .WithDamageTypes(damageTypes)
                .WithInteractionTag(interacitonTag)
                .Build();

            weaponPoolList.Add(mele);
            mele.gameObject.SetActive(false);
            //Events
            OnAttack += MeleAttack;
            OnExitBattleState +=()=> { 
               foreach(var mele in weaponPoolList) {
                    mele.gameObject.SetActive(false);
               }
            };
        }

        //Projectile
        if (weaponType == WeaponTypeEnum.projectile) {
            weaponPoolList = new List<WeaponType>(5);
            for (int i = 0; i < weaponPoolList.Count; i++) {
                var projecitle = new Projectile.ProjectileBuilder(prefab)
                .WithLiveDuration(attackDuration)
                .WithLiveDuration(ProjectileliveDuration)
                .WithDamageTypes(damageTypes)
                .WithInteractionTag(interacitonTag)
                .WithSpeed(projectileSpeed)
                .Build();

                weaponPoolList.Add(projecitle);
                projecitle.gameObject.SetActive(false);
            }

            //Events
            OnAttack += ShootProjectile;
        }
    }
    private void Awake() {
        OnChangeDurationSkill += skillDuration => skillDurationCooldown = skillDuration;
    }
    private void Update() {
        if (skillDurationCooldown > 0) skillDurationCooldown -= Time.deltaTime;

        if (skillDurationCooldown <= 0)
            skillDurationCooldown = 0;

    }

    public void TryAttack() {
        if (skillDurationCooldown > 0) { return; }
        OnAttack?.Invoke();
        OnAnimationStart?.Invoke(animationHash);
        OnChangeDurationSkill?.Invoke(damageDelay); //Here need give him time to start his attack
    }
    public void OnFixedUpdate() {
        TryAttack();
    }
    public void ExitBalltle() {
        OnExitBattleState.Invoke(); 
        cts?.Cancel();
    }
    async void MeleAttack() {
        //UniTask
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        try {

            await UniTask.WaitForSeconds(damageDelay, false, PlayerLoopTiming.Update, token, true);
            foreach (var mele in weaponPoolList) {
                mele.gameObject.SetActive(true);
                OnChangeDurationSkill?.Invoke(attackDuration);         //if attack was done, set coolDown

            }

            await UniTask.WaitForSeconds(0.05f, false, PlayerLoopTiming.Update, token, true);
            foreach (var mele in weaponPoolList) {
                mele.gameObject.SetActive(false);
            }

        } catch (OperationCanceledException) {


        } finally {
            cts.Dispose();
            cts = null;
        }
    }
    void ShootProjectile() {
        foreach (var weapon in weaponPoolList) {
            if (!weapon.gameObject.activeSelf) {
                weapon.gameObject.SetActive(true);
                break;
            }
        }
        
    }
}