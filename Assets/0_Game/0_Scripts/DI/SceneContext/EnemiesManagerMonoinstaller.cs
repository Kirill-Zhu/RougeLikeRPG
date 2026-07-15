using UnityEngine;
using Zenject;

public class EnemiesManagerMonoinstaller : MonoInstaller
{
    [SerializeField] EnemiesManager enemiesManager;
    public override void InstallBindings() {
        Container.Bind<EnemiesManager>().FromInstance(enemiesManager);
    }
}
