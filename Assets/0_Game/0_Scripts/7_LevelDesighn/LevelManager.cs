using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class LevelManager : MonoBehaviour {

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
    EventBinding<OnPlayerDied> onPlayerDied;
    EventBinding<OnPlayerRessurect> onPlayeRessurectBinding;

    private void Awake() {
        HeroSpawner.hero = hero;
        ActivateScene();

        //Events
        OnGamePause.AddListener(hero.OnGamePaused);
        OnGameResume.AddListener(hero.OnGameResume);

        hero.OnLevelUp.AddListener(_ => PauseGame());
        hero.OnChooseLelvelUpCard.AddListener(ResumeGame);
        hero.OnPickUpItemPowerUp.AddListener((_, _, _) => PauseGame());
        OnGameResume.Invoke();
    }

    private void OnEnable() {
        onPlayerDied = new EventBinding<OnPlayerDied>(PauseGame);
        EventBus<OnPlayerDied>.Register(onPlayerDied);

        onPlayeRessurectBinding = new EventBinding<OnPlayerRessurect>(ResumeGame);
        EventBus<OnPlayerRessurect>.Register(onPlayeRessurectBinding);
    }

    private void OnDisable() {
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
        HeroSpawner.SpawnHero(mazeGenerator.PlayerSpawnPointTransform());
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
