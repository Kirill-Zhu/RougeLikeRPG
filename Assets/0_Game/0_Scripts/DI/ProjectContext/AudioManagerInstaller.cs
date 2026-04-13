using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{
    [SerializeField] AudioManager audioManager;
    [Header("References")]
    [SerializeField] EventManager eventManager;
    public override void InstallBindings() {
        AudioManager scenesManagerObject = Container.InstantiatePrefabForComponent<AudioManager>(audioManager, Vector3.zero, Quaternion.identity, null);
        Container.Bind<AudioManager>().FromInstance(scenesManagerObject).AsSingle().NonLazy();
        var audioManagerInstance = scenesManagerObject.GetComponent<AudioManager>();
        audioManagerInstance.SetEventManager(eventManager);
        scenesManagerObject.gameObject.SetActive(true);
    }
}

