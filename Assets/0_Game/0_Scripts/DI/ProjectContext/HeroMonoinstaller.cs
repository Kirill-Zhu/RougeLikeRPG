using UnityEngine;
using Zenject;

public class HeroMonoinstaller : MonoInstaller
{
    [SerializeField] GameObject heroPrefab;
    public override void InstallBindings()
    {

        Hero hero = Container.InstantiatePrefabForComponent<Hero>(heroPrefab, Vector3.zero, Quaternion.identity, null);
        Container.Bind<Hero>().FromInstance(hero).AsSingle().Lazy();
        //hero.gameObject.SetActive(false);
    }
}