using UnityEngine;
using Zenject;

public class GlobalVolumeManagerMonoinstaller : MonoInstaller {
    [SerializeField] GlobalVolumeManager GlobalVolumeManager;
    public override void InstallBindings()
    {
        Container.Bind<GlobalVolumeManager>().FromInstance(GlobalVolumeManager);
    }
}