using UnityEngine;
using Zenject;

public class LevelStatisticsInstaller : MonoInstaller
{
    [SerializeField] LevelStatistics levelStatistics;

    public override void InstallBindings() {
        Container.Bind<LevelStatistics>().FromInstance(levelStatistics);
    }
}
