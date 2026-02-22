using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class LevelManager : MonoBehaviour {
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

        hero.OnLevelUp.AddListener(PauseGame);
        hero.OnChooseLelvelUpCard.AddListener(ResumeGame);
    }
    public void ActivateScene() {
        HeroSpawner.SpawnHero();
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
