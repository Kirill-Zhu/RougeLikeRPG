using UnityEngine;
using Zenject;

public class EventManagerMonoinstaller : MonoInstaller
{
    [SerializeField] EventManager eventManager;
    public override void InstallBindings()
    {
        Container.Bind<EventManager>().FromInstance(eventManager);
    }
}