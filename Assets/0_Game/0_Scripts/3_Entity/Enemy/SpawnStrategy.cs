using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/EntitySpawner/SpawnerStratygy", fileName = "New Spawner Strategy")]
public class SpawnStrategy : ScriptableObject {
    [SerializeField] public EnemyStrategy enemyStrategy;
    public float spawnRate = 1;
    float timer = 0;
    public Action<EnemyStrategy> OnSpawnEntity;

    public void Initialize(Action<EnemyStrategy> @createByType) {
        OnSpawnEntity = @createByType;
    }

    public void OnUpdate(float deltaTime) {

        if (timer < spawnRate) {
            timer += deltaTime;
            return;
        }

        timer = 0;
        OnSpawnEntity.Invoke(enemyStrategy);
    }
}