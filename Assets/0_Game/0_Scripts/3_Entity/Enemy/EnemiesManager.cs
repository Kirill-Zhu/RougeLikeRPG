using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour {
    Hero Hero;
    EventManager eventManager;
    LevelStatistics LevelStatistics;

    public bool Active { get; private set; }
    [SerializeField] int currentWave = 0;
    //SpawnStrategy
    float timer;
    [SerializeField] List<int> wavesTimeAncorsList;
    public List<EnemyStrategyhandler> spawnhandlerList;
    public List<Entity> enemiesOnScene = new List<Entity>();
    [SerializeField] List<Transform> spawnPosList;
    public float NearToHeroDistance = 5;
    public float NearHeroSpeedModifier = 0.8f;
    public string interactionTagName = "Player";
    //Job

    public List<Transform> transforms = new List<Transform>();

    TransformAccessArray transformAccessArray;
    NativeArray<float> speedNativeArray;
    NativeArray<float> attackRangeNativeArray;
    NativeArray<bool> UninterruptedAttackArray;
    NativeArray<float> previousVolocityNativeArray;
    NativeArray<float> returnVelocityNativeArray;
    NativeArray<bool> returnBattleStatusNativeArray;
    JobHandle jobHandle;
    //Event Bus

    [Inject]
    public void Construct(Hero hero, EventManager eventManager, LevelStatistics stats) {
        this.Hero = hero;
        this.eventManager = eventManager;
        this.LevelStatistics = stats;
    }
    private void Awake() {
        eventManager.SetUpEnemiesManager(this);
    }
    public void StartNewSession() {
        Active = true;
        timer = 0;
        currentWave = 0;
        ChangeWave(currentWave);
    }
    void ChangeWave(int currentWave) {
        spawnhandlerList[currentWave].InitializeStrategy(CreateByType);
        EventBus<OnChangeWave>.Raise(new OnChangeWave { wave = currentWave });
    }
    public void IsActive(bool value) {
        Active = value;
    }
    public void CreateByType(EnemyStrategy strategy) {
        Type type = Type.GetType(strategy.TypeOfEnemy); //Create instance Of Type

        var obj = new Entity.TypeBuilder(strategy.prefab, strategy.HealtData, LevelStatistics)
           .WithIcon(strategy.Icon)
           .WithMoveSpeed(strategy.MoveSpeed)
           .WithAttackRange(strategy.AttackRange)
           .WithAttackDuration(strategy.AttackCooldown)
           .WithDamageDelay(strategy.DamageDelay)
           .WithWeaponPrefab(strategy.WeaponPrefab)
           .WithUninterruptedAttack(strategy.UninterruptedAttack)
           .WithWeaponType(strategy.WeaponType)
           .WithDamageTypes(strategy.GetDamageTypes())
           .WithProjecitlieSPeed(strategy.ProjectileSpeed)
           .WithProjectileLiveDuration(strategy.ProjectilieLiveDureation)
           .WithShootShape((int)strategy.ShootShape)
           .WithSpreadAngle(strategy.SpreadAngle)
           .ProjectilesCountByShoot(strategy.ProjecitlesCountByShoot)
           .SelfDirecredProjectile(strategy.SelfDirecrtedProjectile)
           .SetProjectileAim(Hero.transform)
           .WithInteractionTag(interactionTagName)
           .WithDropObject(strategy.DropPfreabList)
           .WithOnAttackParticle(strategy.OnAttackParticelPrefab)
           .WithSounds(strategy.OnAttack, strategy.OnDie)
           .Build(type);

        var component = obj.GetComponent(type);

        //Set pos
        obj.transform.position = spawnPosList[Random.Range(0, spawnPosList.Count)].position.WithY(0);


        if (component is Entity) {

            var entity = (Entity)component;
            enemiesOnScene.Add(entity);
            entity.InitializeEvents(DestroyEnemy);
            transforms.Add(component.transform);

            RefreshAllocations();
        }

    }

    public void DestroyEnemy(Entity entity) {
        enemiesOnScene.Remove(entity);

        transforms.Remove(entity.transform);
        jobHandle.Complete();

        if (enemiesOnScene.Count == 0) return;

        RefreshAllocations();
        //Dispose
        //if (transformAccessArray.isCreated) transformAccessArray.Dispose();
        //if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();
        //if (speedNativeArray.IsCreated) speedNativeArray.Dispose();
        //if (UninterruptedAttackArray.IsCreated) UninterruptedAttackArray.Dispose()


        //transformAccessArray = new TransformAccessArray(transforms.ToArray());

        ////Speed
        ////Speed
        //var speedList = new List<float>();
        //for (int i = 0; i < enemiesOnScene.Count; i++) {
        //    speedList.Add(enemiesOnScene[i].MoveSpeed);
        //}

        //speedNativeArray = new NativeArray<float>(speedList.ToArray(), Allocator.Persistent);

        ////Attack 
        ////->Unitrrupted Attack
        //List<bool> uniterruptedList = new List<bool>();
        //for (int i = 0; i < enemiesOnScene.Count; i++) {
        //    uniterruptedList.Add(enemiesOnScene[i].UninterruptedAttack);
        //}
        //UninterruptedAttackArray = new NativeArray<bool>(uniterruptedList.ToArray(), Allocator.Persistent);

        ////->Attack range
        //var attackRangeList = new List<float>();

        //for (int i = 0; i < enemiesOnScene.Count; i++)
        //    attackRangeList.Add(enemiesOnScene[i].AttackRange);

        //attackRangeNativeArray = new NativeArray<float>(attackRangeList.ToArray(), Allocator.Persistent);
    }

    void RefreshAllocations() {
        if (transformAccessArray.isCreated) transformAccessArray.Dispose();
        if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();
        if (speedNativeArray.IsCreated) speedNativeArray.Dispose();
        if (UninterruptedAttackArray.IsCreated) UninterruptedAttackArray.Dispose();
        if (returnBattleStatusNativeArray.IsCreated) returnBattleStatusNativeArray.Dispose();

        //Speed
        var speedList = new List<float>();
        for (int i = 0; i < enemiesOnScene.Count; i++) {
            speedList.Add(enemiesOnScene[i].MoveSpeed);
        }


        //Attack 
        //->Uniiterrupted Attack
        List<bool> uniterruptedList = new List<bool>();
        for (int i = 0; i < enemiesOnScene.Count; i++) {
            uniterruptedList.Add(enemiesOnScene[i].UninterruptedAttack);
        }

        //->Attack Range
        var attackRangeList = new List<float>();
        for (int i = 0; i < enemiesOnScene.Count; i++) {
            attackRangeList.Add(enemiesOnScene[i].AttackRange);
        }
        //Return Battle Status
        var returnBattleStatus = new List<bool>();
        for (int i = 0; i < enemiesOnScene.Count; i++) {
            returnBattleStatus.Add(enemiesOnScene[i].UninterruptedAttack);
        }
        //Allocate NativeArrays
        transformAccessArray = new TransformAccessArray(transforms.ToArray());
        speedNativeArray = new NativeArray<float>(speedList.ToArray(), Allocator.Persistent);
        UninterruptedAttackArray = new NativeArray<bool>(uniterruptedList.ToArray(), Allocator.Persistent);
        attackRangeNativeArray = new NativeArray<float>(attackRangeList.ToArray(), Allocator.Persistent);
        returnBattleStatusNativeArray = new NativeArray<bool>(returnBattleStatus.ToArray(), Allocator.Persistent);
    }

    private void Update() {

        timer += Time.deltaTime;
        //Wave controll
        if (Active && timer > wavesTimeAncorsList[currentWave]) {
            currentWave++;
            ChangeWave(currentWave);
        }

        //Spawn Strategy
        if (Active && enemiesOnScene.Count < 90) {  //Set limit enemies on scene
            spawnhandlerList[currentWave].Update(Time.deltaTime);
        }

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

        returnBattleStatusNativeArray = new NativeArray<bool>(enemiesOnScene.Count, Allocator.TempJob);
        //----------------------------------------
        MoveEnemyJob moveJob = new MoveEnemyJob() {
            DeltaTime = Time.deltaTime,
            SpeedArray = speedNativeArray,
            MovePoint = Hero.transform.position,
            NearToHeroDistance = NearToHeroDistance,
            NearHeroSpeedModifier = NearHeroSpeedModifier,
            AttackRangeArray = attackRangeNativeArray,
            UniterruptedAttackArray = UninterruptedAttackArray,
            PrevoiusVelocityArray = previousVolocityNativeArray,
            VelocityNativeArray = returnVelocityNativeArray,
            ReturnBattleStatusArray = returnBattleStatusNativeArray

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
                enemiesOnScene[i].InBattle = returnBattleStatusNativeArray[i];                                                      //-> Set BattleStatus to Entities
            }

            if (previousVolocityNativeArray.IsCreated) previousVolocityNativeArray.Dispose();
            if (returnVelocityNativeArray.IsCreated) returnVelocityNativeArray.Dispose();
            if (returnBattleStatusNativeArray.IsCreated) returnBattleStatusNativeArray.Dispose();
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

        if (returnBattleStatusNativeArray.IsCreated) returnBattleStatusNativeArray.Dispose();

    }
    private void OnDisable() {

        //Native Attays
        if (transformAccessArray.isCreated) transformAccessArray.Dispose();

        if (speedNativeArray.IsCreated) speedNativeArray.Dispose();

        if (attackRangeNativeArray.IsCreated) attackRangeNativeArray.Dispose();

        if (returnVelocityNativeArray.IsCreated) returnVelocityNativeArray.Dispose();

        if (returnBattleStatusNativeArray.IsCreated) returnBattleStatusNativeArray.Dispose();
    }
}
[BurstCompile]
public struct MoveEnemyJob : IJobParallelForTransform {

    [ReadOnly] public float DeltaTime;
    [ReadOnly] public NativeArray<float> SpeedArray;
    [ReadOnly] public float NearToHeroDistance;
    [ReadOnly] public float NearHeroSpeedModifier;
    [ReadOnly] public NativeArray<float> AttackRangeArray;
    [ReadOnly] public NativeArray<bool> UniterruptedAttackArray;
    [ReadOnly] public Vector3 MovePoint;
    [ReadOnly] public NativeArray<float> PrevoiusVelocityArray;

    [WriteOnly] public NativeArray<float> VelocityNativeArray;
    public NativeArray<bool> ReturnBattleStatusArray;

    public void Execute(int index, TransformAccess transform) {

        if (Vector3.Distance(MovePoint.WithY(0), transform.position.WithY(0)) < 0.2f) {                                                                       //Dont Rotate entity if its to close
            ReturnBattleStatusArray[index] = true;                                                                                           //Set InBattleStatus true 
            VelocityNativeArray[index] = 0;
            return;                                                                                                                          //Return if entity in attack range
        }

        transform.rotation = Quaternion.LookRotation(MovePoint.WithY(0) - transform.position.WithY(0), Vector3.up);
        Quaternion currentRotation = transform.rotation;

        if (Vector3.Distance(MovePoint, transform.position) < AttackRangeArray[index]) {
            ReturnBattleStatusArray[index] = true;                                                                                           //Set InBattleStatus true
            VelocityNativeArray[index] = 0;
            return;                                                                                                                          //Return if entity in attack range
        }
        if (Vector3.Distance(MovePoint, transform.position) > AttackRangeArray[index]) {
            //To not evenry time reset attack status need this threshold of 1f
            if (ReturnBattleStatusArray[index] == true && UniterruptedAttackArray[index] == true) {
                ReturnBattleStatusArray[index] = true;
            } else
                ReturnBattleStatusArray[index] = false;
        }

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
