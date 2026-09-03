using BossEntity;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    EventInstance music;
    [SerializeField] EventReference standartMusic;
    [SerializeField] EventReference bossFightMusic;
    [SerializeField] EventReference dieMenu;

    [Header("One shots")]
    [SerializeField] EventReference coinSound;
    [SerializeField] EventReference changeWaveSound;
    //Dependencies
    [SerializeField] EventManager eventManager;

    //->Event Bus
    EventBinding<OnUpgradeItemInShop> onUpgradeItemInShopBinding;
    EventBinding<OnChangeWave> onChangeWaveBinding;
    EventBinding<OnSpawnBoss> onSpawnBossBinding;
    EventBinding<OnPlayerDied> onPlayerDiedBinding;
    EventBinding<OnPlayerRessurect> onPlayerRessurectBinding;



    private void OnEnable() {
        //Shop
        onUpgradeItemInShopBinding = new EventBinding<OnUpgradeItemInShop>(PlayCoinSound);
        EventBus<OnUpgradeItemInShop>.Register(onUpgradeItemInShopBinding);
        //Wave
        onChangeWaveBinding = new EventBinding<OnChangeWave>(PlayChangeWaveSound);
        EventBus<OnChangeWave>.Register(onChangeWaveBinding);
        //Boss
        onSpawnBossBinding = new EventBinding<OnSpawnBoss>(StartBossFightMusic);
        EventBus<OnSpawnBoss>.Register(onSpawnBossBinding);

        // Player Die
        onPlayerDiedBinding = new EventBinding<OnPlayerDied>(StartDieMenuMusic);
        EventBus<OnPlayerDied>.Register(onPlayerDiedBinding);
        //Player Ressurect
        onPlayerRessurectBinding = new EventBinding<OnPlayerRessurect>(StartPlayStandartMusic);
        EventBus<OnPlayerRessurect>.Register(onPlayerRessurectBinding);

    }
    private void OnDisable() {
        EventBus<OnUpgradeItemInShop>.Deregister(onUpgradeItemInShopBinding);
        EventBus<OnChangeWave>.Deregister(onChangeWaveBinding);
        EventBus<OnSpawnBoss>.Deregister(onSpawnBossBinding);
        EventBus<OnPlayerDied>.Deregister(onPlayerDiedBinding);
        EventBus<OnPlayerRessurect>.Deregister(onPlayerRessurectBinding);
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
    //-> Event Bus
    public void PlayCoinSound() {
        RuntimeManager.PlayOneShot(coinSound);
    }
    public async void PlayChangeWaveSound() {
        await UniTask.Delay(1000);
        RuntimeManager.PlayOneShot(changeWaveSound);
    }
}
