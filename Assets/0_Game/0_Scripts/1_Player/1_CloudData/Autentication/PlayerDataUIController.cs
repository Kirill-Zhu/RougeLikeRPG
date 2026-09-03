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
    EventBinding<OnChangeData> OnChangeDataBinding;
    private void OnEnable() {
        OnPlayerSighInBinding = new EventBinding<OnPlayerSignIn>(SetPlayerName);
        OnChangeDataBinding = new EventBinding<OnChangeData>(SetData);
        EventBus<OnPlayerSignIn>.Register(OnPlayerSighInBinding);
        EventBus<OnChangeData>.Register(OnChangeDataBinding);
        Debug.Log("Register");
        Refresh();
    }

    private void OnDisable() {
        EventBus<OnPlayerSignIn>.Deregister(OnPlayerSighInBinding);
        EventBus<OnChangeData>.Deregister(OnChangeDataBinding);
    }

     void SetPlayerName(OnPlayerSignIn player) {
        var palyerName = playerDataUIDocument.rootVisualElement.Q<Label>(UserName);
        palyerName.text = player.userName;
        Debug.Log($"{GetType().Name} is SetPlayer name as {player.userName}");

        var conisText = playerDataUIDocument.rootVisualElement.Q<Label> (Conins);
        conisText.text = cloudSaveManager.Coins.ToString();
    }

    void SetData(OnChangeData data) {
        var conisText = playerDataUIDocument.rootVisualElement.Q<Label>(Conins);
        conisText.text = data.Coins.ToString();
    }
    void Refresh() {
        var conisText = playerDataUIDocument.rootVisualElement.Q<Label>(Conins);
        conisText.text = cloudSaveManager.Coins.ToString();
    }
}
