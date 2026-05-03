using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class LevelManager : MonoBehaviour {
    [SerializeField] ChoseHeroStrategy heroStrategy;
    [SerializeField] HeroStrategyData heroData;
    [Inject]
    public Hero hero;

    public HeroSpawner HeroSpawner;

    public UnityEvent OnGamePause;
    public UnityEvent OnGameResume;
    private void Awake() {
        HeroSpawner.hero = hero;
        ActivateScene();

        //Events
        OnGamePause.AddListener(hero.OnGamePaused);
        OnGameResume.AddListener(hero.OnGameResume);

        hero.OnLevelUp.AddListener(_ => PauseGame());
        hero.OnChooseLelvelUpCard.AddListener(ResumeGame);
        hero.OnPickUppowerUp.AddListener((_, _, _) => PauseGame());
        hero.OnDie.AddListener(() => PauseGame());

        OnGameResume.Invoke();
    }
    private void OnDestroy() {
        //Events
        OnGamePause.RemoveAllListeners();
        OnGameResume.RemoveAllListeners();

        hero.OnLevelUp.RemoveAllListeners();
        hero.OnChooseLelvelUpCard.RemoveAllListeners();
        hero.OnPickUppowerUp.RemoveAllListeners();
        hero.OnDie.RemoveAllListeners();
    }
    public void ActivateScene() {
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
}
