using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Jobs;
using static UnityEngine.UI.Image;
[CreateAssetMenu(menuName = "SkillStrategy/Shoot", fileName = "Shoot")]
public class ShootStrategy : SkillsStrategy {

    //Shoot Shape
    public override int CurrentAnimationHash { get => Animator.StringToHash(animationName); set => throw new System.NotImplementedException(); }
    public ShootShape shootShape = ShootShape.Forward;
    [Range(0, 360)] public float spreadAngle;
    float angleStep;
    //------------------
    public int projectilesCountByShoot = 1;
    public int damage;
    public float speed;
    public string animationName;
    public const string interactionTagName = "Enemy";
    public float LiveDuration = 10;
    bool initialization;
    List<Projectile> projectiles = new List<Projectile>();
    List<Transform> objectsToMove = new List<Transform>();
    TransformAccessArray transformAccessArray = new TransformAccessArray();
    bool[] activeObjectsArray = new bool[0];
    NativeArray<bool> activeProjectilisNativeArray = new NativeArray<bool>();
    JobHandle jobHandle;
    public override void Initialize(Transform origin, AudioManager audioManager) {
        initialization = true;
        //Add Damage Types it deals(set the values at definitons)
        damageTypesList = GetStartDamageTypes().ToList();

        foreach (var damageType in damageTypesList)
            SetOrAddDamageTypeWithValues(damageType);

        BuildNewProjectiles(origin);

        //Audio
        this.audioManager = audioManager;
    }
    public async override void Dispose() {
        foreach (var projectile in projectiles) {
            Destroy(projectile.gameObject);
            await UniTask.WaitForFixedUpdate();
        }
        damageTypesList.Clear();
        projectiles.Clear();
        objectsToMove.Clear();
        // Dispose of the TransformAccessArray to prevent memory leaks
        if (transformAccessArray.isCreated) {
            transformAccessArray.Dispose();
        }
        //---------------
        if (activeProjectilisNativeArray.IsCreated)
            activeProjectilisNativeArray.Dispose();

    }

   
    public async override void UpdateValues() {
        initialization = true;
        //Dispose 
        foreach (var projectile in projectiles) {
            Destroy(projectile.gameObject);
            await UniTask.WaitForFixedUpdate();
        }
           
        projectiles.Clear();
        objectsToMove.Clear();
        // Dispose of the TransformAccessArray to prevent memory leaks
        if (transformAccessArray.isCreated) {
            transformAccessArray.Dispose();
        }
        //---------------
        if (activeProjectilisNativeArray.IsCreated)
            activeProjectilisNativeArray.Dispose();


        BuildNewProjectiles(Origin);
    }
    async void BuildNewProjectiles(Transform origin) {
        initialization = true;
        angleStep = spreadAngle / projectilesCountByShoot;

        Origin = origin;
        projectiles = new List<Projectile>();
        objectsToMove = new List<Transform>();


        Debug.Log($"Shoot types list count is {damageTypesList.Count}");
        //Object pool
        for (int i = 0; i < 32; i++) {

            var projectile = new Projectile.ProjectileBuilder(prefab)
                .FromOrigin(Origin)
                .WithSpeed(speed)
                .WithDamageTypes(damageTypesList.ToArray())
                .WithInteractionTag(interactionTagName)
                .WithLiveDuration(LiveDuration)
                .Build();

            //Job
            projectile.gameObject.SetActive(false);
            projectiles.Add(projectile);
            objectsToMove.Add(projectile.transform);
            await UniTask.WaitForFixedUpdate();
        }

        activeObjectsArray = new bool[objectsToMove.Count];
        //Job

        transformAccessArray = new TransformAccessArray(objectsToMove.ToArray());
        Debug.Log($"Initialize {GetType().Name}");

        initialization = false;
    }
    public override void OnUpdate(float deltaTime) {
        if (initialization) return;

        if (coolDownTimer > 0) {
            coolDownTimer -= deltaTime;
            InvokeOnCoolDownCall(coolDownTimer / CoolDown);
        }

        for (int i = 0; i < activeObjectsArray.Length; i++) {
            activeObjectsArray[i] = objectsToMove[i].gameObject.activeInHierarchy;
        }
        activeProjectilisNativeArray = new NativeArray<bool>(activeObjectsArray, Allocator.TempJob);

        for (int i = 0; i < projectiles.Count; i++) {
            if (projectiles[i].gameObject.activeInHierarchy)
                projectiles[i].LiveDuration -= deltaTime;

            if (projectiles[i].LiveDuration <= 0) {
                projectiles[i].gameObject.SetActive(false);
            }
        }

        ////Job
        MoveJob moveJob = new MoveJob() {
            deltaTime = deltaTime,
            speed = speed,
            activeNativeArray = activeProjectilisNativeArray,
        };
        jobHandle = moveJob.Schedule(transformAccessArray);
        jobHandle.Complete();

        if (activeProjectilisNativeArray.IsCreated)
            activeProjectilisNativeArray.Dispose();
    }
    void OnDestroy() {
        //Dispose 

        foreach (var projectile in projectiles)
            Destroy(projectile.gameObject);

        projectiles.Clear();
        objectsToMove.Clear();
        // Dispose of the TransformAccessArray to prevent memory leaks
        if (transformAccessArray.isCreated) {
            transformAccessArray.Dispose();
        }
        if (activeProjectilisNativeArray.IsCreated)
            activeProjectilisNativeArray.Dispose();
    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int, float> OnAnimation, UnityAction<int> OnManaChange) {

        Debug.Log("shoot");
        OnChangeSkillDuration.Invoke(SkillDuration);

        if (coolDownTimer > 0) return;
        OnAnimation?.Invoke(CurrentAnimationHash, SkillDuration);
        OnManaChange?.Invoke(-ManaCost);
        ShootProjectile();
    }


    void ShootProjectile() {
        //Audio
        audioManager.PlayOneShot(SkillSound, Origin.position);
        
        //Shape
        switch (shootShape) {
            case ShootShape.Forward: {
                    for (int i = 0; i < projectilesCountByShoot; i++) {

                        foreach (var projectile in projectiles) {

                            if (!projectile.gameObject.activeSelf) {
                                projectile.gameObject.SetActive(true);
                                projectile.transform.position = Origin.position;
                                projectile.transform.rotation = Origin.rotation;
                                projectile.LiveDuration = LiveDuration;
                                coolDownTimer = CoolDown;
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
                        foreach (var projectile in projectiles) {

                            if (!projectile.gameObject.activeSelf) {
                                projectile.gameObject.SetActive(true);
                                projectile.transform.position = Origin.position;
                                projectile.transform.forward = direction;
                                projectile.LiveDuration = LiveDuration;
                                coolDownTimer = CoolDown;
                                break;
                            }
                        }
                    }
                    break;
                }

            case ShootShape.Spread: {
                    for (int i = 0; i < projectilesCountByShoot; i++) {


                        Vector3 direction;
                        if (i != 0 && i % 2 == 0) {
                            float angle = 0 + (i - 1) * angleStep;
                            direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

                        } else {
                            float angle = 0 + i * angleStep;
                            direction = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
                        }

                        direction = Origin.TransformDirection(direction);
                        foreach (var projectile in projectiles) {

                            if (!projectile.gameObject.activeSelf) {
                                projectile.gameObject.SetActive(true);
                                projectile.transform.position = Origin.position;
                                projectile.transform.forward = direction;
                                projectile.LiveDuration = LiveDuration;
                                coolDownTimer = CoolDown;
                                break;
                            }
                        }
                    }
                    break;
                }
        }


    }


    [BurstCompile]
    public struct MoveJob : IJobParallelForTransform {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public float speed;
        [ReadOnly] public NativeArray<bool> activeNativeArray;
        public void Execute(int index, TransformAccess transform) {

            if (activeNativeArray[index] == false) return;
            // Get the current world rotation as a Quaternion
            Quaternion currentRotation = transform.rotation;

            // Calculate the local forward direction in world space
            // This is the equivalent of transform.forward in a regular MonoBehaviour
            Vector3 forwardDirection = currentRotation * Vector3.forward; //

            transform.position += forwardDirection * speed * deltaTime;
        }
    }

}

