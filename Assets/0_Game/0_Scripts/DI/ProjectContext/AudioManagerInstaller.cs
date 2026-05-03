using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{
    [SerializeField] AudioManager audioManager;
    [Header("References")]
    [SerializeField] EventManager eventManager;
    [SerializeField] HeroMonoinstaller heroMoninstaller;
    [SerializeField] ScenesManagerMonoinstaller scenesManagerMonoinstaller;
    public override void InstallBindings() {
        AudioManager audio = Container.InstantiatePrefabForComponent<AudioManager>(audioManager, Vector3.zero, Quaternion.identity, null);
        Container.Bind<AudioManager>().FromInstance(audio).AsSingle().NonLazy();
        var audioManagerInstance = audio.GetComponent<AudioManager>();
        
        //Set event manager
        audioManagerInstance.SetEventManager(eventManager);
        
        //Set hero
        audioManagerInstance.SetHeroMonoinstaller(heroMoninstaller);
        audioManagerInstance.GetHeroReference();
        //Set scenes manager
        audioManagerInstance.SetScenesManagerMonoinstaller(scenesManagerMonoinstaller);
        audioManagerInstance.GetScenesManagerReference();

        audio.gameObject.SetActive(true);
    }
}

