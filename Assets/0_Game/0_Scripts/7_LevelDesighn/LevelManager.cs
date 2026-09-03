using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class LevelManager : MonoBehaviour {

    [SerializeField] int levelProgressKey;
    [Header ("Test map Generator options")]
    [SerializeField] MazeGenerator mazeGenerator;

    [Header ("End Test")]
    [SerializeField] ChoseHeroStrategy heroStrategy;
    [SerializeField] HeroStrategyData heroData;
    [Inject]
    public Hero hero;

    public HeroSpawner HeroSpawner;

    public UnityEvent OnGamePause;
    public UnityEvent OnGameResume;

    //Event Bus 
    EventBinding<OnPlayerEndLevel> onPlayerEndLevelBinding;
    EventBinding<OnPlayerDied> onPlayerDied;
    EventBinding<OnPlayerRessurect> onPlayeRessurectBinding;

    private void Awake() {
        HeroSpawner.hero = hero;
        ActivateScene();
        mazeGenerator.Initialize(levelProgressKey);
        //Events
        OnGamePause.AddListener(hero.OnGamePaused);
        OnGameResume.AddListener(hero.OnGameResume);

        hero.OnLevelUp.AddListener(_ => PauseGame());
        hero.OnChooseLelvelUpCard.AddListener(ResumeGame);
        hero.OnPickUpItemPowerUp.AddListener((_, _, _) => PauseGame());
        OnGameResume.Invoke();
    }

    private void OnEnable() {


        //EventBus
        onPlayerEndLevelBinding = new EventBinding<OnPlayerEndLevel>(PauseGame);
        EventBus<OnPlayerEndLevel>.Register(onPlayerEndLevelBinding);

        onPlayerDied = new EventBinding<OnPlayerDied>(PauseGame);
        EventBus<OnPlayerDied>.Register(onPlayerDied);

        onPlayeRessurectBinding = new EventBinding<OnPlayerRessurect>(ResumeGame);
        EventBus<OnPlayerRessurect>.Register(onPlayeRessurectBinding);

    }
    private async void Start() {

        await UniTask.WaitForSeconds(1);
        //EventBus Raise Events
        EventBus<OnPlayerStartLevel>.Raise(new OnPlayerStartLevel() { levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name });
    }
    private void OnDisable() {
        EventBus<OnPlayerEndLevel>.Deregister(onPlayerEndLevelBinding);
        EventBus<OnPlayerDied>.Deregister(onPlayerDied);
        EventBus<OnPlayerRessurect>.Deregister(onPlayeRessurectBinding);
    }
    private void OnDestroy() {
        //Events
        OnGamePause.RemoveAllListeners();
        OnGameResume.RemoveAllListeners();

        hero.OnLevelUp.RemoveAllListeners();
        hero.OnChooseLelvelUpCard.RemoveAllListeners();
        hero.OnPickUpItemPowerUp.RemoveAllListeners();
    }
    public void ActivateScene() {
        if (HeroSpawner.SpawnPoint == null) {
            HeroSpawner.SpawnHero(mazeGenerator.PlayerSpawnPointTransform());
        } else
            HeroSpawner.SpawnHero();

        heroData = heroStrategy.HeroStrategyData;
        hero.Initialize(heroData);
    }
    public void PauseGame() {
        Time.timeScale = 0;
        OnGamePause?.Invoke();
    }
    public void ResumeGame() {
        Time.timeScale = 1f;
        OnGameResume?.Invoke();
    }


    [ContextMenu("Safe zone Raise Event")]
    public void SetSafeZone() {
        EventBus<OnSafeZone>.Raise(new OnSafeZone() { Hero = this.hero});
    }
}
