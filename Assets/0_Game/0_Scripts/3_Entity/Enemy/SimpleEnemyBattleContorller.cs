using Cysharp.Threading.Tasks;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class SimpleEnemyBattleContorller : MonoBehaviour {
    //Initialize Values
    float attackDuration;
    float damageDelay;
    WeaponTypeEnum weaponType;

    //SHooter
    [SerializeField] ShootShape shootShape = ShootShape.Spread;
    float spreadAngle = 75;
    int projectilesCountByShoot = 3;
    bool selfDirected = false;
    Transform aimTransform;
    float angleStep;
    string interacitonTag;
    float attackRange;

    public List<WeaponType> weaponPoolList = new List<WeaponType>();

    //For Porjectiles
    public List<Projectile> projectileList = new List<Projectile>();
    float liveDuration = 1;
    public event Action OnAttack;
    event Action OnExitBattleState;

    //For Area
    public List<AreaWeaponType> areaWeaponList = new List<AreaWeaponType>();
    float areaLiveDuration = 3;

    //-------------------

    //VFX
    [SerializeField] GameObject onAttackParticlePrefab;
    [SerializeField] ParticleSystem onAttackVFX;

    //Sound
    EventReference onAttackSound;
    [SerializeField] float skillDurationCooldown;
    public event Action<int> OnAnimationStart;
    public event Action<float> OnChangeDurationSkill;


    //Animations
    int animationHash = Animator.StringToHash("Battle");

    //UniTask
    CancellationTokenSource cts;

    public void Initialize(float attackDuration
        , WeaponTypeEnum weaponType
        , float attackRange
        , float damageDelay
        , DamageType[] damageTypes
        , GameObject prefab
        , string interactionTag = null
        , float projectileSpeed = 2
        , float ProjectileliveDuration = 2,
        int shootShape = 0
        , float spreadAngle = 70
        , int projectilesCount = 1
        , bool isProjectileSelfDirected = false
        , Transform aimTransform = null
        , GameObject onAttackParticlePrefab = null
        , EventReference onAttackSound = new EventReference()) {

        this.attackDuration = attackDuration;
        this.weaponType = weaponType;
        this.attackRange = attackRange;
        this.damageDelay = damageDelay;
        this.interacitonTag = interactionTag;
        this.shootShape = (ShootShape)shootShape;
        this.spreadAngle = spreadAngle;
        this.projectilesCountByShoot = projectilesCount;
        this.selfDirected = isProjectileSelfDirected;
        this.aimTransform = aimTransform;
        this.onAttackParticlePrefab = onAttackParticlePrefab;
        this.onAttackSound = onAttackSound;

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
            OnExitBattleState += () => {
                foreach (var mele in weaponPoolList) {
                    mele.gameObject.SetActive(false);
                }
            };
        }

        //Projectile
        angleStep = this.spreadAngle / projectilesCountByShoot;
        if (weaponType == WeaponTypeEnum.projectile) {
            weaponPoolList = new List<WeaponType>();
            for (int i = 0; i < 5; i++) {
                var projecitle = new Projectile.ProjectileBuilder(prefab)
                    .FromOrigin(transform)
                    .WithLiveDuration(attackDuration)
                    .WithLiveDuration(ProjectileliveDuration)
                    .WithDamageTypes(damageTypes)
                    .WithInteractionTag(interacitonTag)
                    .WithSpeed(projectileSpeed)
                    .SelfDirected(selfDirected)
                    .Build();

                projectileList.Add(projecitle);
                projecitle.gameObject.SetActive(false);
                this.liveDuration = ProjectileliveDuration;
            }
            //Events
            OnAttack += ShootProjectile;
        }
        if (weaponType == WeaponTypeEnum.area) {
            weaponPoolList = new List<WeaponType>();

            var area = new AreaWeaponType.Builder(prefab)
                .FromOrigin(transform)
                .WithDamageTypes(damageTypes)
                .WithLiveDuration(areaLiveDuration)
                .WithInteractionTag(interactionTag)
                .FollowCaster(false)
                .DestroyAfterUse(false)
                .Build();

            areaWeaponList.Add(area);
            area.gameObject.SetActive(false);

            //Events
            OnAttack += CastArea;
        }

        //VFX
        var vfxObj = Instantiate(this.onAttackParticlePrefab, null);
        vfxObj.TryGetComponent<ParticleSystem>(out ParticleSystem particle);
        onAttackVFX = particle;
    }
    public void OnDie() {
        cts?.Cancel();
    }
    private void Awake() {
        OnChangeDurationSkill += skillDuration => skillDurationCooldown = skillDuration;
    }
    private void Update() {
        if (skillDurationCooldown > 0) skillDurationCooldown -= Time.deltaTime;

        if (skillDurationCooldown <= 0)
            skillDurationCooldown = 0;


        //Move Porjectiles
        if (projectileList.Count > 0) {
            foreach (var projectile in projectileList)
                if (projectile.gameObject.activeInHierarchy) {
                    if (projectile.SelfDirected)
                        projectile.transform.Translate((aimTransform.position - projectile.transform.position).normalized * Time.deltaTime * projectile.Speed, Space.World);
                    else
                        projectile.transform.Translate(projectile.transform.forward * Time.deltaTime * projectile.Speed, Space.World);


                    projectile.LiveDuration -= Time.deltaTime;
                    if (projectile.LiveDuration <= 0) projectile.gameObject.SetActive(false);
                }
        }
    }
    private void OnDestroy() {
        cts?.Cancel();
        foreach (var projectile in projectileList) {
            if (projectile != null)
                Destroy(projectile.gameObject);
        }

        //VFX
        if (onAttackVFX != null)
            Destroy(onAttackVFX.gameObject);
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
        OnExitBattleState?.Invoke();
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

            //VFX
            if (onAttackVFX != null) {
                onAttackVFX.gameObject.transform.position = transform.position;
                onAttackVFX.gameObject.transform.rotation = Quaternion.LookRotation(aimTransform.position.WithY(0) - transform.position.WithY(0));
                onAttackVFX.Play();
            }

            //Sound 
            RuntimeManager.PlayOneShot(onAttackSound, transform.position);


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
    async void ShootProjectile() {
        //UniTask
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        try {
            await UniTask.WaitForSeconds(damageDelay, false, PlayerLoopTiming.Update, token, true);
            OnChangeDurationSkill?.Invoke(attackDuration);

            //VFX
            if (onAttackVFX != null) {
                onAttackVFX.gameObject.transform.position = transform.position;
                onAttackVFX.Play();
            }

            //Sound 
            RuntimeManager.PlayOneShot(onAttackSound, transform.position);

            switch (shootShape) {
                case ShootShape.Forward: {
                        for (int i = 0; i < projectilesCountByShoot; i++) {

                            foreach (var projectile in projectileList) {

                                if (!projectile.gameObject.activeSelf) {
                                    projectile.gameObject.SetActive(true);
                                    projectile.transform.position = transform.position + Vector3.up;
                                    projectile.transform.rotation = transform.rotation;
                                    projectile.LiveDuration = liveDuration;
                                    return;
                                }
                            }
                        }
                        break;
                    }
                case ShootShape.Around: {

                        for (int i = 0; i < projectilesCountByShoot; i++) {

                            float angle = 0 + i * angleStep;
                            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
                            foreach (var projectile in projectileList) {

                                if (!projectile.gameObject.activeSelf) {
                                    projectile.gameObject.SetActive(true);
                                    projectile.transform.position = transform.position + Vector3.up;
                                    projectile.transform.forward = direction;
                                    projectile.LiveDuration = liveDuration;
                                    break;
                                }
                            }
                        }
                        break;
                    }

                case ShootShape.Spread: {
                        for (int i = 0; i < projectilesCountByShoot; i++) {

                            float angle = 0 + i * angleStep;
                            Vector3 direction;
                            if (i % 2 == 0)
                                direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
                            else
                                direction = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

                            direction = transform.TransformDirection(direction);
                            foreach (var projectile in projectileList) {

                                if (!projectile.gameObject.activeSelf) {
                                    projectile.gameObject.SetActive(true);
                                    projectile.transform.position = transform.position + Vector3.up;
                                    projectile.transform.forward = direction;
                                    projectile.LiveDuration = liveDuration;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
        } catch (OperationCanceledException) {

        } finally {
            cts?.Cancel();
            cts = null;
        }


    }
    async void CastArea() {
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        Vector3 castPos = aimTransform.position.WithY(0);
        //Sound 
        RuntimeManager.PlayOneShot(onAttackSound, transform.position);

        //VFX (First Cast VFX)
        if (onAttackVFX != null) {
            onAttackVFX.gameObject.transform.position = castPos;
            onAttackVFX.Play();
        }

        //Sound 
        RuntimeManager.PlayOneShot(onAttackSound, transform.position);

        try {

            await UniTask.WaitForSeconds(damageDelay, false, PlayerLoopTiming.Update, token, true);
            foreach (var area in areaWeaponList) {
                area.gameObject.SetActive(true);
                area.transform.position = castPos;
                OnChangeDurationSkill?.Invoke(attackDuration);         //if attack was done, set coolDown
            }

            await UniTask.WaitForSeconds(areaLiveDuration, false, PlayerLoopTiming.Update, token, true);
            foreach (var area in areaWeaponList) {
                area.gameObject.SetActive(false);
            }

        } catch (OperationCanceledException) {


        } finally {
            cts.Dispose();
            cts = null;
        }


    }
}
