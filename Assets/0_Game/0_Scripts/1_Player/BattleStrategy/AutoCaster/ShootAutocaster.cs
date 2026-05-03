using Cysharp.Threading.Tasks;
using FMODUnity;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;



[CreateAssetMenu(menuName = "SkillStrategy/AutoCaster/Shoot", fileName = "new Shoot Autocaster")]
public class ShootAutocaster : AutoSkillStrategy {

    [SerializeField] int projectilesCountByShoot = 1;
    [SerializeField] ShootShape shootShape = ShootShape.Forward;
    [SerializeField, Range(0, 360)] float spreadAngle;
    float angleStep;
    [SerializeField] float speed = 1;
    [SerializeField] float LiveDuration = 1;
    bool initialization;
    List<Projectile> projectiles = new List<Projectile>();
    List<Transform> objectsToMove = new List<Transform>();
    TransformAccessArray transformAccessArray = new TransformAccessArray();
    bool[] activeObjectsArray = new bool[0];
    NativeArray<bool> activeProjectilisNativeArray = new NativeArray<bool>();
    JobHandle jobHandle;
    public override async void Initialize(Transform origin) {
        initialization = true;


        Origin = origin;
        projectiles = new List<Projectile>();
        objectsToMove = new List<Transform>();

        //Spread Angle
        angleStep = spreadAngle / projectilesCountByShoot;

        //Add Damage Types it deals(set the values at definitons)
        damageTypesList = GetStartDamageTypes().ToList();

        Debug.Log($"Shoot types list count is {damageTypesList.Count}");
        //Object pool
        for (int i = 0; i < 100; i++) {

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
            InvokeOnCoolDownCall(coolDownTimer / coolDown);
        } else {
            UseSKill();
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


    protected override void UseSKill() {

        //VFX
        foreach (var particle in particleSystemArray) {
            particle.Play();
        }

        ShootProjectiles();
        PlayOnCastSound();
    }

    void ShootProjectiles() {

        switch (shootShape) {
            case ShootShape.Forward: {
                    for (int i = 0; i < projectilesCountByShoot; i++) {

                        foreach (var projectile in projectiles) {

                            if (!projectile.gameObject.activeSelf) {
                                projectile.gameObject.SetActive(true);
                                projectile.transform.position = Origin.position;
                                projectile.transform.rotation = Origin.rotation;
                                projectile.LiveDuration = LiveDuration;
                                coolDownTimer = coolDown;
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
                                coolDownTimer = coolDown;
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
                        if(i % 2 == 0)
                             direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
                        else
                             direction = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

                        direction = Origin.TransformDirection(direction);
                    foreach (var projectile in projectiles) {

                        if (!projectile.gameObject.activeSelf) {
                            projectile.gameObject.SetActive(true);
                            projectile.transform.position = Origin.position;
                            projectile.transform.forward = direction;
                            projectile.LiveDuration = LiveDuration;
                            coolDownTimer = coolDown;
                            break;
                        }
                    }
                }
                break;
        }    
        }


    }

    protected override void PlayOnCastSound() {
        RuntimeManager.PlayOneShot(OnCastSound, Origin.position);
    }

    //Move Job--------------------------------------------
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