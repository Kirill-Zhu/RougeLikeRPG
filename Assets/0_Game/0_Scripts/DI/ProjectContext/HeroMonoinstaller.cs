using UnityEngine;
using Zenject;

public class HeroMonoinstaller : MonoInstaller
{
    [SerializeField] GameObject heroPrefab;
    [SerializeField] EventManager eventManager;
    Hero hero;
    public override void InstallBindings()
    {
        Hero hero = Container.InstantiatePrefabForComponent<Hero>(heroPrefab, Vector3.zero, Quaternion.identity, null);
        this.hero = hero;
        hero.SetEventManager(eventManager);
        Container.Bind<Hero>().FromInstance(hero).AsSingle().Lazy();
        //hero.gameObject.SetActive(false);
    }
    public Hero GetHero() {
        return hero;
    }
}