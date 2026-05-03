using UnityEngine;
using Zenject;

public class ScenesManagerMonoinstaller : MonoInstaller
{
    [SerializeField] ScenesManager scenesManager;
    ScenesManager scensManager;
    [SerializeField] EventManager eventManager;
    public override void InstallBindings()
    {
        ScenesManager scenesManagerObject = Container.InstantiatePrefabForComponent<ScenesManager>(scenesManager, Vector3.zero, Quaternion.identity, null);
        scenesManager = scenesManagerObject;
        scenesManager.SetEventManager(eventManager);

        Container.Bind<ScenesManager>().FromInstance(scenesManagerObject).AsSingle().NonLazy();
        scenesManagerObject.gameObject.SetActive(true);
    }

    public ScenesManager GetScenesManager() {
        return scenesManager;   
    }
}