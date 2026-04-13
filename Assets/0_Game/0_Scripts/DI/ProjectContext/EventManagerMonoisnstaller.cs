using UnityEngine;
using Zenject;

public class EventManagerMonoisnstaller : MonoInstaller
{
    [SerializeField] EventManager eventManager;
    public override void InstallBindings()
    {
        Container.Bind<EventManager>().FromInstance(eventManager).NonLazy();
    }
}