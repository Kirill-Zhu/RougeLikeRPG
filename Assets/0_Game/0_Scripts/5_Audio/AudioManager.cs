using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Zenject;
using BossEntity;

public class AudioManager : MonoBehaviour
{
    EventInstance music;
    [SerializeField] EventReference standartMusic;
    [SerializeField] EventReference bossFightMusic;
    [SerializeField] EventReference dieMenu;

    [Header("One shots")]
    [SerializeField] EventReference coinSound;

    //Dependencies
    [SerializeField] EventManager eventManager;
    //Events
    HeroMonoinstaller heroMonoinstaller;
    //->Event Bus
    EventBinding<OnUpgradeItemInShop> onUpgradeItemInShop;
    EventBinding<OnSpawnBoss> onSpawnBoss;
    EventBinding<OnPlayerDied> onPlayerDied;
    //Hero
    Hero hero;
    //ScenesManager
    ScenesManagerMonoinstaller scenesManagerMonoinstaller;
    [SerializeField] ScenesManager scenesManager;

    private void Start() {
       
    }

    private void OnEnable() {
        //Shop
        onUpgradeItemInShop = new EventBinding<OnUpgradeItemInShop>(PlayCoinSound);
        EventBus<OnUpgradeItemInShop>.Register(onUpgradeItemInShop);
        //Boss
        onSpawnBoss = new EventBinding<OnSpawnBoss>(StartBossFightMusic);
        EventBus<OnSpawnBoss>.Register(onSpawnBoss);

        // Player Die
        onPlayerDied = new EventBinding<OnPlayerDied>(StartDieMenuMusic);
        EventBus<OnPlayerDied>.Register(onPlayerDied);
    }
    private void OnDisable() {
        EventBus<OnUpgradeItemInShop>.Deregister(onUpgradeItemInShop);
        EventBus<OnSpawnBoss>.Deregister(onSpawnBoss);
        EventBus<OnPlayerDied>.Deregister(onPlayerDied);
    }

    void OnLoadAppPlay() {
        music = RuntimeManager.CreateInstance(standartMusic);
        music.start();
    }
    public void PlayMusic(EventReference eventReference) {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(eventReference);
        music.start();
    }
    public void StartPlayStandartMusic() {
        Debug.Log("Play standart music");
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(standartMusic);
        music.start();
        //RuntimeManager.PlayOneShot(reference, new Vector3(38,0,66));
    }
    public void PlayOneShot(EventReference reference, Vector3 pos) {
        RuntimeManager.PlayOneShot(reference, pos);
    }
 //Boss
    public void StartBossFightMusic(OnSpawnBoss @event) {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(bossFightMusic);
        music.start();
    }
    public void StartBossFightMusic(Boss boss) {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(bossFightMusic);
        music.start();
    }
    [ContextMenu("Die music")]

    //Die
    public void StartDieMenuMusic() {
        Debug.Log("Start die music");
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(dieMenu);
        music.start();

    }
    //Events
    public void SetEventManager(EventManager eventManager) { 
       this.eventManager = eventManager;
       
    }

    //-> Event Bus
     public void PlayCoinSound(OnUpgradeItemInShop @event) {
        RuntimeManager.PlayOneShot(coinSound);
    }
    //
    //Hero
   public void SetHeroMonoinstaller(HeroMonoinstaller monoinstaller) {
        this.heroMonoinstaller = monoinstaller; 
    }
    public void GetHeroReference() {
        if (eventManager == null) return;
        hero = heroMonoinstaller.GetHero();
    }

    //Scenes
    public void SetScenesManagerMonoinstaller(ScenesManagerMonoinstaller scenesManagerMonoinstaller) { 
        this.scenesManagerMonoinstaller = scenesManagerMonoinstaller;
       
    }

    public void GetScenesManagerReference() {
        if(scenesManagerMonoinstaller == null) return;  
        scenesManager = scenesManagerMonoinstaller.GetScenesManager();
        eventManager.OnLoadMainMenu.AddListener(StartPlayStandartMusic);
    }
}
