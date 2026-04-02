using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{
    [SerializeField] AudioManager audioManager;
    public override void InstallBindings() {
        AudioManager scenesManagerObject = Container.InstantiatePrefabForComponent<AudioManager>(audioManager, Vector3.zero, Quaternion.identity, null);
        Container.Bind<AudioManager>().FromInstance(scenesManagerObject).AsSingle().NonLazy();
        scenesManagerObject.gameObject.SetActive(true);
    }
}

