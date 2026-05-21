using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDataUIController : MonoBehaviour
{
    [SerializeField] AuthentificationManager authentificationManager;
    [SerializeField] UIDocument playerDataUIDocument;
    const string UserName = "UserName";


    EventBinding<OnPlayerSignIn> OnPlayerSighInBinding;
    private void Awake() {
        OnPlayerSighInBinding = new EventBinding<OnPlayerSignIn>(SetPlayerName);
        EventBus<OnPlayerSignIn>.Register(OnPlayerSighInBinding);
        Debug.Log("Register");
    }

    private void OnDisable() {
        EventBus<OnPlayerSignIn>.Deregister(OnPlayerSighInBinding);
    }

     void SetPlayerName(OnPlayerSignIn player) {
        Debug.Log($"{GetType().Name} is SetPlayer name");
        var palyerName = playerDataUIDocument.rootVisualElement.Q<Label>(UserName);
        palyerName.text = player.userName;
    }
}
