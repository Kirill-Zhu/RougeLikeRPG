using UnityEngine;
using Zenject;

public class FireBaseMonoinstaller : MonoInstaller
{
    [SerializeField] FireBaseManager fireBaseManager;
    public override void InstallBindings() {
        Container.Bind<FireBaseManager>().FromInstance(fireBaseManager);
    }
}
