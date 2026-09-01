using Cysharp.Threading.Tasks;
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
    [SerializeField] int maxEnemiesOnScene;
    [Header("Testing")]
    [SerializeField] Transform[] posList;
    public bool Active { get; private set; }
    [SerializeField] int currentWave = 0;
    //SpawnStrategy
    float timer;
    [SerializeField] List<int> wavesTimeAncorsList;
    public List<EnemyStrategyTier> tiersList;
    HashSet<EnemyStrategyTier> activeTiersList = new HashSet<EnemyStrategyTier>();
    public List<Entity> enemiesOnScene = new List<Entity>();
    [SerializeField] List<Transform> spawnPosList;
    public float NearToHeroDistance = 5;
    public float NearHeroSpeedModifier = 0.8f;
    public string interactionTagName = "Player";
    float[] velArray = new float[100];

    //Damage Buffer
    
    [SerializeField] DamageBuffer damageBuffer;

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

    [Inject]
    public void Construct(Hero hero, EventManager eventManager, LevelStatistics stats) {
        this.Hero = hero;
        this.eventManager = eventManager;
        this.LevelStatistics = stats;
    }
    private void Awake() {
        eventManager.SetUpEnemiesManager(this);

        //roomsSpawnhandlerList[0].InitializeStrategy(CreateByType);
        //for (int i = 0; i < 100; i++) {
        //    roomsSpawnhandlerList[0].strategies[0].Spawn();
        //}

        //Testting
      // StartSession();
    }
    public void StartNewSession() {
        Active = true;
        timer = 0;
        currentWave = 0;
        // ChangeWave(currentWave);
    }
    void ChangeWave(int currentWave) {
        //tiersList[currentWave].InitializeStrategy(CreateByType);
        EventBus<OnChangeWave>.Raise(new OnChangeWave { wave = currentWave });
    }

    [ContextMenu("Start test spawn")]
   public void StartSession() {
        StartSpawnSession(0, posList);
    }
    public async void StartSpawnSession(int tierIndex, Transform[] posList) {
        //Сделать индексацию комнат по tierIndex
        var tier = Instantiate(tiersList[tierIndex]);
        tier.InitializeStrategy(CreateByType, posList);
        await UniTask.Delay(200);
        activeTiersList.Add(tier);
    }

    public void IsActive(bool value) {
        Active = value;
    }
    void CreateByType(EnemyStrategy strategy) {
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
    void CreateByType(EnemyStrategy strategy, Transform[] positionsArray) {
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
        //Health
        var health = obj.gameObject.GetComponent<HealthComponent>();
        health.InitializeDamageBuffer(damageBuffer);
        //Set pos
        obj.transform.position = positionsArray[Random.Range(0, positionsArray.Length)].position.WithY(0);


        if (component is Entity) {

            var entity = (Entity)component;
            enemiesOnScene.Add(entity);
            entity.InitializeEvents(DestroyEnemy);
            transforms.Add(component.transform);

            RefreshAllocations();
        }

    }
    void DestroyEnemy(Entity entity) {
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

        if (activeTiersList.Count > 0)
            foreach (var room in activeTiersList) {
                room.OnUpdate(Time.deltaTime);
            }
        //timer += Time.deltaTime;
        //Wave controll
        //if (Active && timer > wavesTimeAncorsList[currentWave]) {
        //    currentWave++;
        //    ChangeWave(currentWave);
        //}

        ////Spawn Strategy
        //if (Active && enemiesOnScene.Count < 90) {  //Set limit enemies on scene
        //    spawnhandlerList[currentWave].Update(Time.deltaTime);
        //}

        //Move Job
        if (enemiesOnScene.Count <= 0) return;

        //Velocity--------------------------------
        if (velArray.Length != enemiesOnScene.Count) {
            velArray = new float[enemiesOnScene.Count];
        }
        for (int i = 0; i < velArray.Length; i++) {
            velArray[i] = enemiesOnScene[i].VelocityMagnitude;
        }
        previousVolocityNativeArray = new NativeArray<float>(velArray, Allocator.TempJob);

        returnVelocityNativeArray = new NativeArray<float>(enemiesOnScene.Count, Allocator.TempJob);
        //----------------------------------------
        //Battle Status --------------------------

        returnBattleStatusNativeArray = new NativeArray<bool>(enemiesOnScene.Count, Allocator.TempJob);
        //----------------------------------------
        MoveAndBattleJob moveJob = new MoveAndBattleJob() {
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

        foreach(var activeTierSpawner in activeTiersList) {
            Destroy(activeTierSpawner);
        }
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
[BurstCompile(CompileSynchronously = true)]
public struct MoveAndBattleJob : IJobParallelForTransform {

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
        Vector3 currentPos = transform.position;
        Vector3 targetPosSameY = MovePoint.WithY(0);
        Vector3 currentPosSameY = currentPos.WithY(0);

        float distanceToTargetSameY = Vector3.Distance(targetPosSameY, currentPosSameY);

        // 1. Close to hero chesk
        if (distanceToTargetSameY < 0.2f) {
            ReturnBattleStatusArray[index] = true;
            VelocityNativeArray[index] = 0;
            return;
        }

        // Rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetPosSameY - currentPosSameY, Vector3.up);

        float distanceToTarget = Vector3.Distance(MovePoint, currentPos);

        // 2. Distance Check
        if (distanceToTarget < AttackRangeArray[index]) {
            ReturnBattleStatusArray[index] = true;
            VelocityNativeArray[index] = 0;
            return;
        }

        if (distanceToTarget >= AttackRangeArray[index] + 0.1f) {
            if (ReturnBattleStatusArray[index] == true && UniterruptedAttackArray[index] == true) {
                ReturnBattleStatusArray[index] = true;
            } else {
                ReturnBattleStatusArray[index] = false;
            }
        }

        // Forward direction
        Vector3 forwardDirection = targetRotation * Vector3.forward;
        forwardDirection.y = 0f;

        float speedModifier = 1f;
        if (distanceToTarget < NearToHeroDistance) {
            speedModifier = NearHeroSpeedModifier;
        }

        // generate new pos
        Vector3 nextPosition = currentPos + (forwardDirection * DeltaTime * SpeedArray[index] * speedModifier);


        transform.SetPositionAndRotation(nextPosition, targetRotation);

        // Вычисляем Velocity
        float velocity = Mathf.Lerp(PrevoiusVelocityArray[index], speedModifier, DeltaTime);
        VelocityNativeArray[index] = velocity;
    }
}
