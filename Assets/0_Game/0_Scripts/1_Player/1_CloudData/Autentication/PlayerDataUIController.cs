using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class PlayerDataUIController : MonoBehaviour
{
    [Inject] CloudSaveManager cloudSaveManager;
    [SerializeField] AuthentificationManager authentificationManager;
    [SerializeField] UIDocument playerDataUIDocument;
    const string UserName = "UserName";
    const string Conins = "Coins";

    EventBinding<OnPlayerSignIn> OnPlayerSighInBinding;
    private void OnEnable() {
        OnPlayerSighInBinding = new EventBinding<OnPlayerSignIn>(SetPlayerData);
        EventBus<OnPlayerSignIn>.Register(OnPlayerSighInBinding);
        Debug.Log("Register");
    }

    private void OnDisable() {
        EventBus<OnPlayerSignIn>.Deregister(OnPlayerSighInBinding);
    }

     void SetPlayerData(OnPlayerSignIn player) {
        var palyerName = playerDataUIDocument.rootVisualElement.Q<Label>(UserName);
        palyerName.text = player.userName;
        Debug.Log($"{GetType().Name} is SetPlayer name as {player.userName}");

        var conisText = playerDataUIDocument.rootVisualElement.Q<Label> (Conins);
        conisText.text = cloudSaveManager.Coins.ToString();
    }
}
