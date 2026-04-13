using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UIElements.Button;
public class FirsButtonSelecedHandler : MonoBehaviour {
    [Inject] ScenesManager scenesManager;
    [SerializeField] List<UIDocument> UidocumentsList;
    [SerializeField] ChoseHeroController chooseHeroController;

    //Main menu
    const string MainMenu = "MainMenu";
    const string ChooseHeroButton = "ChooseHeroButton";

    //Chose hero menu
    const string ChooseHeroMenu = "ChoseHeroMenu";
    const string StartGameMenu = "StartGameMenu";
    const string NextHero = "NextHero";
    const string PrevHero = "PreviousHero";
    const string StartGame = "StartGame";
    private void Awake() {

        OpenMainMenu();
    }

    private async void Start() {

        await UniTask.Delay(100);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    void OpenMainMenu() {
        //-----Main Menu
        var button = UidocumentsList[0].rootVisualElement.Q<Button>(ChooseHeroButton);
        button.Focus();
        EventSystem.current.SetSelectedGameObject(gameObject);
        button.clicked += OpenChooseHeroMenu;

        //-----Chose Hero Menu
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        choseHeroMenu.style.display = DisplayStyle.None;
        startGameMenu.style.display = DisplayStyle.None;
    }
    void OpenChooseHeroMenu() {

        //-----Main Menu
        var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        mainMenu.style.display = DisplayStyle.None;

        //-----Chose Hero Menu
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        var nextHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(NextHero);
        var prevHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(PrevHero);
        var startButton = UidocumentsList[1].rootVisualElement.Q<Button>(StartGame);

        //Visability
        choseHeroMenu.style.display = DisplayStyle.Flex;
        startGameMenu.style.display = DisplayStyle.Flex;
        nextHeroButton.Focus();

        //Events
        nextHeroButton.clicked += ChooseNextHero;
        prevHeroButton.clicked += ChoosePrevHero;
        startButton.clicked += ChoseHeroAndStartGame;
    }

    void ChooseNextHero() {
        Debug.Log("Next");
        chooseHeroController.NextModel();
    }
    void ChoosePrevHero() {
        Debug.Log("Prev");
        chooseHeroController.PreviousModel();
    }

    void ChoseHeroAndStartGame() {
        scenesManager.StartGame();
    }
}
