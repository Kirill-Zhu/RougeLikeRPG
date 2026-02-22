using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
[CreateAssetMenu(menuName = "SkillStrategy/Shoot", fileName = "Shoot")]
public class ShootStrategy : SkillsStrategy {
    public override int CurrentAnimationHash { get => Animator.StringToHash(animationName); set => throw new System.NotImplementedException(); }

    [SerializeField] int physicsDamage;
    [SerializeField] int fireDamage;
    [SerializeField] int coldDamage;
    public int damage;
    public float speed;
    public string animationName;
    public const string interactionTagName = "Enemy";
    public float LiveDuration = 10;
    bool initialization;
    List<Projectile> projectiles = new List<Projectile>();
    List<Transform> objectsToMove = new List<Transform>();
    TransformAccessArray transformAccessArray = new TransformAccessArray();
    JobHandle jobHandle;
    public override async void Initialize(Transform origin) {

      initialization = true;

        Origin = origin;
        projectiles = new List<Projectile>();
        objectsToMove = new List<Transform>();


        //Add Damage Types it deals(set the values at definitons)
        damageTypesArray = GetDamageTypes();
        //Object pool
        for (int i = 0; i < 32; i++) {

            var projectile = new Projectile.ProjectileBuilder(prefab)
                .FromOrigin(Origin)
                .WithSpeed(speed)
                .WithDamageTypes(damageTypesArray)
                .WithInteractionTag(interactionTagName)
                .WithLiveDuration(LiveDuration)
                .Build();

            //Job
            projectile.gameObject.SetActive(false);
            projectiles.Add(projectile);
            objectsToMove.Add(projectile.transform);
            await UniTask.WaitForFixedUpdate();
        }
        //Job
        transformAccessArray = new TransformAccessArray(objectsToMove.ToArray());
        Debug.Log($"Initialize {GetType().Name}");

        initialization = false;
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

        Initialize(Origin);

    }

    public override void OnUpdate(float deltaTime) {
        if (initialization) return;

        if (coolDownTimer > 0) {
            coolDownTimer -= deltaTime;
            InvokeOnCoolDownCall(coolDownTimer/CoolDown);
        }
        for (int i = 0; i < projectiles.Count; i++) {
            if(projectiles[i].gameObject.activeInHierarchy) 
                projectiles[i].LiveDuration -= deltaTime;

            if (projectiles[i].LiveDuration <= 0) {
                projectiles[i].gameObject.SetActive(false);
            }
        }

        ////Job
        MoveJob moveJob = new MoveJob() {
            deltaTime = deltaTime,
            speed = speed,
        };
        jobHandle = moveJob.Schedule(transformAccessArray);
        jobHandle.Complete();
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

    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int, float> OnAnimation) {

            OnChangeSkillDuration.Invoke(SkillDuration);
        
        if (coolDownTimer > 0) return;
        OnAnimation.Invoke(CurrentAnimationHash, SkillDuration);
        ShootProjectile();
    }

    void ShootProjectile() {
      

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

}
[BurstCompile]
public struct MoveJob : IJobParallelForTransform {
    [ReadOnly] public float deltaTime;
    [ReadOnly] public float speed;
    public void Execute(int index, TransformAccess transform) {
        // Get the current world rotation as a Quaternion
        Quaternion currentRotation = transform.rotation;

        // Calculate the local forward direction in world space
        // This is the equivalent of transform.forward in a regular MonoBehaviour
        Vector3 forwardDirection = currentRotation * Vector3.forward; //

        transform.position += forwardDirection * speed * deltaTime;
    }
}