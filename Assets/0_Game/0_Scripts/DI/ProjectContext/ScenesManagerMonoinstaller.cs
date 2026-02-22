using UnityEngine;
using Zenject;

public class ScenesManagerMonoinstaller : MonoInstaller
{
    [SerializeField] ScenesManager scenesManager;
    public override void InstallBindings()
    {
        ScenesManager scenesManagerObject = Container.InstantiatePrefabForComponent<ScenesManager>(scenesManager, Vector3.zero, Quaternion.identity, null);
        Container.Bind<ScenesManager>().FromInstance(scenesManagerObject).AsSingle().NonLazy();
        scenesManagerObject.gameObject.SetActive(true);
    }
}