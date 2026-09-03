using UnityEngine;
using Zenject;

public class CloudSaveMaangerMoniinstaller : MonoInstaller
{
    [SerializeField] CloudSaveManager saveManager;
    public override void InstallBindings() {
        Container.Bind<CloudSaveManager>().FromInstance(saveManager);
    }
}
