using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Strategy/Enemy/EnemyStratgyHandler", fileName = "EnemyTypes")]
public class EnemyStrategyTier : ScriptableObject {
    [SerializeField] int currentWave;
    [SerializeField] public List<SpawnStrategy> strategies;
    Transform[] positionsList;
    public void InitializeStrategy(Action<EnemyStrategy, Transform[]> @enemy, Transform[] posList) {
        positionsList = posList;
        foreach (var strategy in strategies) {
            strategy.Initialize(@enemy, positionsList);
        }

    }

    public void OnUpdate(float deltaTime) {
        foreach (var strategy in strategies) {
            strategy.OnUpdate(deltaTime);
        }
    }
}
