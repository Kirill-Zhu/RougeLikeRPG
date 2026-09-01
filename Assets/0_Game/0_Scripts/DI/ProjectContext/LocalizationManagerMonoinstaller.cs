using UnityEngine;
using Zenject;

public class LocalizationManagerMonoinstaller : MonoInstaller
{
    [SerializeField] LocalizationManager localizationManger;
    public override void InstallBindings() {
        Container.Bind<LocalizationManager>().FromInstance(localizationManger);
    }
}
