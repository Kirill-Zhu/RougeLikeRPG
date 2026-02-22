using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;


public class EnemiesManager : MonoBehaviour {
    [Inject]
    public Hero Hero;
    public EnemyStrategyhandler enemyTypes;
    public List<Entity> enemiesOnScene = new List<Entity>();
    [SerializeField] List<Vector3> spawnPosList;
    public float NearToHeroDistance = 5;
    public float NearHeroSpeedModifier = 0.8f;
    public string interactionTagName = "Player";
    //Job

    public List<Transform> transforms = new List<Transform>();

    TransformAccessArray transformAccessArray;
    NativeArray<float> speedNativeArray;
    NativeArray<float> attackRangeNativeArray;
    NativeArray<float> previousVolocityNativeArray;
    NativeArray<float> returnVelocityNativeArray;
    NativeArray<bool> returnBattleStatus;
    JobHandle jobHandle;
    private void Awake() {
        //InitializeSpawnPoints
        for (int i = 1; i < 400; i++) {
            spawnPosList.Add(new Vector3(i, 0, 0));
        }
    }
    private void Start() {
        //--------------Spawn Enemies------------///
        for (int i = 0; i < spawnPosList.Count; i++) {
            CreateByType(typeof(Dummy), enemyTypes.commonSkelet, spawnPosList[i]);
        }
    }
    public void CreateByType(Type type, EnemyStrategy strategy, Vector3 pos) {

        var obj = new Entity.TypeBuilder(strategy.prefab, strategy.HealtData)
           .WithIcon(strategy.Icon)
           .WithMoveSpeed(strategy.MoveSpeed)
           .WithAttackRange(strategy.AttackRange)
           .WithAttackDuration(strategy.AttackDuration)
           .WithDamageDelay(strategy.DamageDelay)
           .WithWeaponPrefab(strategy.WeaponPrefab)
           .WithWeaponType(strategy.WeaponType)
           .WithDamageTypes(strategy.GetDamageTypes())
           .WithInteractionTag(interactionTagName)
           .WithDropObject(strategy.DropPrefab)
           .Build(type);

        var component = obj.GetComponent(type);

        //Set pos
        obj.transform.position = pos;


        if (component is Entity) {

            var entity = (Entity)component;
            enemiesOnScene.Add(entity);
            entity.InitializeEvents(DestroyEnemy);
            transforms.Add(component.transform);

            //Job
            //Transforms
            if (transformAccessArray.isCreated) transformAccessArray.Dispose();

            transformAccessArray = new TransformAccessArray(transforms.ToArray());
            //Speed
            var speedList = new List<float>();
            for (int i = 0; i < enemiesOnScene.Count; i++) {
                speedList.Add(enemiesOnScene[i].MoveSpeed);
            }
            if (speedNativeArray.IsCreated) speedNativeArray.Dispose();

            speedNativeArray = new NativeArray<float>(speedList.ToArray(), Allocator.Persistent);
            //Attack Range
            var attackRangeList = new List<float>();
            for (int i = 0; i < enemiesOnScene.Count; i++) {
                attackRangeList.Add(enemiesOnScene[i].AttackRange);
            }

            if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();

            attackRangeNativeArray = new NativeArray<float>(attackRangeList.ToArray(), Allocator.Persistent);

        }

    }

    public void DestroyEnemy(Entity entity) {
        enemiesOnScene.Remove(entity);

        transforms.Remove(entity.transform);
        jobHandle.Complete();

        if (transformAccessArray.isCreated) transformAccessArray.Dispose();
        if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();
        if (speedNativeArray.IsCreated) speedNativeArray.Dispose();


        if (enemiesOnScene.Count == 0) return;

        transformAccessArray = new TransformAccessArray(transforms.ToArray());

        //Speed
        var speedList = new List<float>();

        for (int i = 0; i < enemiesOnScene.Count; i++)
            speedList.Add(enemiesOnScene[i].MoveSpeed);

        speedNativeArray = new NativeArray<float>(speedList.ToArray(), Allocator.Persistent);

        //Attack Range
        var attackRangeList = new List<float>();

        for (int i = 0; i < enemiesOnScene.Count; i++)
            attackRangeList.Add(enemiesOnScene[i].AttackRange);

        attackRangeNativeArray = new NativeArray<float>(attackRangeList.ToArray(), Allocator.Persistent);
    }
    private void Update() {
        //Move Job
        if (enemiesOnScene.Count <= 0) return;

        //Velocity--------------------------------

        float[] velArray = new float[enemiesOnScene.Count];
        for (int i = 0; i < velArray.Length; i++) {
            velArray[i] = enemiesOnScene[i].VelocityMagnitude;
        }
        previousVolocityNativeArray = new NativeArray<float>(velArray, Allocator.TempJob);

        returnVelocityNativeArray = new NativeArray<float>(enemiesOnScene.Count, Allocator.TempJob);
        //----------------------------------------
        //Battle Status --------------------------

        returnBattleStatus = new NativeArray<bool>(enemiesOnScene.Count, Allocator.TempJob);
        //----------------------------------------
        MoveEnemyJob moveJob = new MoveEnemyJob() {
            DeltaTime = Time.deltaTime,
            SpeedArray = speedNativeArray,
            MovePoint = Hero.transform.position,
            NearToHeroDistance = NearToHeroDistance,
            NearHeroSpeedModifier = NearHeroSpeedModifier,
            AttackRangeArray = attackRangeNativeArray,
            PrevoiusVelocityArray = previousVolocityNativeArray,
            VelocityNativeArray = returnVelocityNativeArray,
            ReturnBattleStatusArray = returnBattleStatus

        };
        jobHandle = moveJob.Schedule(transformAccessArray);

    }

    private void LateUpdate() {


        //end job
        jobHandle.Complete();

        //Set Result Values To Entities
        if (jobHandle.IsCompleted) {


            for (int i = 0; i < enemiesOnScene.Count; i++) {
                enemiesOnScene[i].VelocityMagnitude = returnVelocityNativeArray[i];                                      //-> Set VelocityMagnitude to Entities
                enemiesOnScene[i].InBattle = returnBattleStatus[i];                                                      //-> Set BattleStatus to Entities
            }

            if (previousVolocityNativeArray.IsCreated) previousVolocityNativeArray.Dispose();
            if (returnVelocityNativeArray.IsCreated) returnVelocityNativeArray.Dispose();
            if (returnBattleStatus.IsCreated) returnBattleStatus.Dispose();
        }

    }

    private void OnDestroy() {

        enemiesOnScene.Clear();
        transforms.Clear();

        //Native Attays
        if (transformAccessArray.isCreated) transformAccessArray.Dispose();

        if (speedNativeArray.IsCreated) speedNativeArray.Dispose();

        if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();

        if (returnVelocityNativeArray.IsCreated) returnVelocityNativeArray.Dispose();

        if (returnBattleStatus.IsCreated) returnBattleStatus.Dispose();

    }
    private void OnDisable() {

        //Native Attays
        if (transformAccessArray.isCreated) transformAccessArray.Dispose();

        if (speedNativeArray.IsCreated) speedNativeArray.Dispose();

        if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();

        if (returnVelocityNativeArray.IsCreated) returnVelocityNativeArray.Dispose();

        if (returnBattleStatus.IsCreated) returnBattleStatus.Dispose();
    }
}
[BurstCompile]
public struct MoveEnemyJob : IJobParallelForTransform {

    [ReadOnly] public float DeltaTime;
    [ReadOnly] public NativeArray<float> SpeedArray;
    [ReadOnly] public float NearToHeroDistance;
    [ReadOnly] public float NearHeroSpeedModifier;
    [ReadOnly] public NativeArray<float> AttackRangeArray;
    [ReadOnly] public Vector3 MovePoint;
    [ReadOnly] public NativeArray<float> PrevoiusVelocityArray;

    [WriteOnly] public NativeArray<float> VelocityNativeArray;
    [WriteOnly] public NativeArray<bool> ReturnBattleStatusArray;

    public void Execute(int index, TransformAccess transform) {

        if (Vector3.Distance(MovePoint.WithY(0), transform.position.WithY(0)) < 0.2f) {                                                                       //Dont Rotate entity if its to close
            ReturnBattleStatusArray[index] = true;                                                                                           //Set InBattleStatus true 
            return;                                                                                                                          //Return if entity in attack range
        }

        transform.rotation = Quaternion.LookRotation(MovePoint.WithY(0) - transform.position.WithY(0), Vector3.up);
        Quaternion currentRotation = transform.rotation;

        if (Vector3.Distance(MovePoint, transform.position) < AttackRangeArray[index]) {

            ReturnBattleStatusArray[index] = true;                                                                                           //Set InBattleStatus true
            return;                                                                                                                          //Return if entity in attack range
        }
        ReturnBattleStatusArray[index] = false;
        // Calculate the local forward direction in world space
        // This is the equivalent of transform.forward in a regular MonoBehaviour
        Vector3 forwardDirection = currentRotation * Vector3.forward;
        forwardDirection.y = 0f;

        float speedModifier = 1;
        if (Vector3.Distance(transform.position, MovePoint) < NearToHeroDistance)
            speedModifier = NearHeroSpeedModifier;

        transform.position += forwardDirection * DeltaTime * SpeedArray[index] * speedModifier;
        //find Velocity
        float velocity = Mathf.Lerp(PrevoiusVelocityArray[index], speedModifier, DeltaTime);
        VelocityNativeArray[index] = velocity;


    }
}
