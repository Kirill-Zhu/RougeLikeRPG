using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/EntitySpawner/SpawnerStratygy", fileName = "New Spawner Strategy")]
public class SpawnStrategy : ScriptableObject {
    [SerializeField] public EnemyStrategy enemyStrategy;
    public float SpawnTime = 10;
    float spawnTimer = 0;
    public float spawnRate = 2;
    float cooldownTimer = 0;

    Transform[] posArray;
    public Action<EnemyStrategy, Transform[]> OnSpawnEntity;

    public void Initialize(Action<EnemyStrategy, Transform[]> @createByType, Transform[] posArray) {
        spawnTimer = 0;
        cooldownTimer = 0;
        this.posArray = posArray;
        OnSpawnEntity = @createByType;
    }

    public void OnUpdate(float deltaTime) {

        spawnTimer += deltaTime;
        if (spawnTimer > SpawnTime) return;
        
        if (cooldownTimer < spawnRate) {
            cooldownTimer += deltaTime;
            return;
        }

        cooldownTimer = 0;
        OnSpawnEntity?.Invoke(enemyStrategy,posArray);
    }
    public void Spawn() {
        OnSpawnEntity?.Invoke(enemyStrategy, posArray);
    }
}