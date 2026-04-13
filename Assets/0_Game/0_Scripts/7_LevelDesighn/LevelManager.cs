using Cysharp.Threading.Tasks;
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
    }
    private async void Start() {
        await UniTask.Delay(1000);
        ActivateScene();
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
