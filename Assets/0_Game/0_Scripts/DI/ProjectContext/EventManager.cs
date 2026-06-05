using BossEntity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
public class EventManager : MonoBehaviour {
    [SerializeField] Hero hero;
    [SerializeField] GlobalVolumeManager volumeManager;
    [SerializeField] ScenesManager scenesManager;
    [SerializeField] InGameUIManager gameUIManager;
    [SerializeField] AudioManager audioManager;
    EnemiesManager enemiesManager;
    [HideInInspector] public UnityEvent<Boss> OnBossCreate;
    [HideInInspector] public UnityEvent OnLoadMainMenu;
    public void RaiseOnBossCreate(Boss boss) {
        Debug.Log($"Raise OnBossCreate {boss.GetType().Name}");
        OnBossCreate?.Invoke(boss);
    }
    public async void LoadMainMenu() {
        Time.timeScale = 1;
        gameUIManager?.HideStatistics();
        gameUIManager?.HideAllUI();
        hero.ExitLevel();
        await volumeManager.FadeOut();
        scenesManager.LoadMainMenu();
    }
    public async void StartNewGame() {
        Time.timeScale = 1;
        if (gameUIManager != null) {
            gameUIManager.HideStatistics();
            gameUIManager.HideAllUI();
        }
        await scenesManager.LoadLevelAsync(ScenesManager.Level2SceneID);
        audioManager.StartPlayStandartMusic();
        await UniTask.Delay(3000);
        volumeManager.FadeIn();
        hero.IsActive.Value = true;
        enemiesManager.StartNewSession();
    }
    public async void LoadLevel(int levelId) {
        Time.timeScale = 1;
        gameUIManager?.HideStatistics();
        gameUIManager?.HideAllUI();
        hero.ExitLevel();
        await volumeManager.FadeOut();

        scenesManager.LoadLevelAsync(levelId);
        audioManager.StartPlayStandartMusic();
    }
    public void SetUpHero(Hero hero) {
        this.hero = hero;
    }
    public void SetUpSceneManager(ScenesManager scenesManager) {
        this.scenesManager = scenesManager;
    }
    public void SetUpAudioManager(AudioManager audioManger) {
        this.audioManager = audioManger;
    }
    public void SetUpImGameUIManager(InGameUIManager gameUIManager) {
        this.gameUIManager = gameUIManager;
    }
    public void SetUpEnemiesManager(EnemiesManager enemiesManager) { 
        this.enemiesManager = enemiesManager;
    }
}
