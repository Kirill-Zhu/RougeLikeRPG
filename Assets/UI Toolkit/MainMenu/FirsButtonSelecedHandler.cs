using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Zenject;
public class FirsButtonSelecedHandler : MonoBehaviour
{
    [Inject] ScenesManager scenesManager;
    [SerializeField] UIDocument Uidocument;
    VisualElement VisualElement;

    private void Awake() {
        VisualElement = Uidocument.rootVisualElement;

        var button = VisualElement.Q<Button>("Start");


        button.Focus();
        EventSystem.current.SetSelectedGameObject(gameObject);
        button.clicked += TestStartGame;
        
    }

    private async void Start() {

        await UniTask.Delay(1000);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    void TestStartGame() {
        scenesManager.TestStarGame();
    }
}
