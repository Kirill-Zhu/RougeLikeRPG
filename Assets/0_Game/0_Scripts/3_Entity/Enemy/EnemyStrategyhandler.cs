using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Strategy/Enemy/EnemyStratgyHandler", fileName = "EnemyTypes")]
public class EnemyStrategyhandler : ScriptableObject {
    [SerializeField] int currentWave;
    [SerializeField] List<SpawnStrategy> strategies;

    public void InitializeStrategy(Action<EnemyStrategy> @enemy) {
        foreach (var strategy in strategies) {
            strategy.Initialize(@enemy);
        }

    }
    public void Update(float deltaTime) {
        foreach (var strategy in strategies) {
            strategy.OnUpdate(deltaTime);
        }
    }
}
