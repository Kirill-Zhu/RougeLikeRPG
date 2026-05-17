using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UIElements.Button;
public class FirsButtonSelecedHandler : MonoBehaviour {
    [Inject] ScenesManager scenesManager;
    [SerializeField] List<UIDocument> UidocumentsList;
    [SerializeField] ChoseHeroController chooseHeroController;
    [SerializeField] ShopManager shopManager;

    //Main menu
    const string MainMenu = "MainMenu";
    const string ChooseHeroButton = "ChooseHeroButton";

    //Chose hero menu
    const string ChooseHeroMenu = "ChoseHeroMenu";
    const string StartGameMenu = "StartGameMenu";
    const string HeroDesk = "HeroDesk";
    const string NextHero = "NextHero";
    const string PrevHero = "PreviousHero";
    const string StartGame = "StartGame";
    const string Shop = "Shop";
    private void Awake() {
        OpenMainMenu();
    }

    private async void Start() {

        await UniTask.Delay(100);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    void OpenMainMenu() {
        //-----Main Menu
        var choseHeroButton = UidocumentsList[0].rootVisualElement.Q<Button>(ChooseHeroButton);
        choseHeroButton.Focus();
        EventSystem.current.SetSelectedGameObject(gameObject);
        choseHeroButton.clicked += OpenChooseHeroMenu;

        //-----Chose Hero Menu
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        var heroDesk = UidocumentsList[1].rootVisualElement.Q<VisualElement>(HeroDesk);
        choseHeroMenu.style.display = DisplayStyle.None;
        startGameMenu.style.display = DisplayStyle.None;
        heroDesk.style.display = DisplayStyle.None;

        //Shop
        var shopButton = UidocumentsList[0].rootVisualElement.Q<Button>(Shop);
        shopButton.clicked += OpenShop;

    }
    void OpenChooseHeroMenu() {
        //Shop
        CloseShop();

        //-----Main Menu
        var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        mainMenu.style.display = DisplayStyle.None;

        //-----Chose Hero Menu
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        var heroDesk = UidocumentsList[1].rootVisualElement.Q<VisualElement>(HeroDesk);
        var nextHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(NextHero);
        var prevHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(PrevHero);
        var startButton = UidocumentsList[1].rootVisualElement.Q<Button>(StartGame);

   
      

        //Visability
        choseHeroMenu.style.display = DisplayStyle.Flex;
        startGameMenu.style.display = DisplayStyle.Flex;
        heroDesk.style.display = DisplayStyle.Flex;
        nextHeroButton.Focus();

        //Events
        nextHeroButton.clicked += ChooseNextHero;
        prevHeroButton.clicked += ChoosePrevHero;
        startButton.clicked += ChoseHeroAndStartGame;

        ChangeHeroDescriptionDesk();
    }
    void CloseChooseHeroMenu() {
        //-----Main Menu
        var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        mainMenu.style.display = DisplayStyle.Flex;

        //-----Chose Hero Menu
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        var nextHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(NextHero);
        var prevHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(PrevHero);
        var startButton = UidocumentsList[1].rootVisualElement.Q<Button>(StartGame);


        //Visability
        choseHeroMenu.style.display = DisplayStyle.None;
        startGameMenu.style.display = DisplayStyle.None;

        //Events
        nextHeroButton.clicked -= ChooseNextHero;
        prevHeroButton.clicked -= ChoosePrevHero;
        startButton.clicked -= ChoseHeroAndStartGame;

        //Focus

    }
    void OpenShop() {
        CloseChooseHeroMenu();
        shopManager.OpenShop();
    }
    void CloseShop() {
        shopManager.CloseShop();
    }
    void ChooseNextHero() {
        Debug.Log("Next");
        chooseHeroController.NextModel();
        ChangeHeroDescriptionDesk();

    }
    void ChoosePrevHero() {
        Debug.Log("Prev");
        chooseHeroController.PreviousModel();
        ChangeHeroDescriptionDesk();
    }
    void ChangeHeroDescriptionDesk() {
        //Hero Desk
        var prevHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(PrevHero);
        //Icon
        var heroIcon = UidocumentsList[1].rootVisualElement.Q<VisualElement>("HeroIcon");
        heroIcon.style.backgroundImage = new StyleBackground(chooseHeroController.CurrentHeroStrategyData.Icon);

        //StoryTell
        var heroStory = UidocumentsList[1].rootVisualElement.Q<Label>("HeroStory");
        heroStory.text = chooseHeroController.CurrentHeroStrategyData.Story;

        //Health
        var healthValue = UidocumentsList[1].rootVisualElement.Q<Label>("HealthValue");
        healthValue.text = $" health {chooseHeroController.CurrentHeroStrategyData.HealtComponentData.MaxHealth.ToString()}";

        //Mana
        var manaValue = UidocumentsList[1].rootVisualElement.Q<Label>("ManaValue");
        manaValue.text = $" mana: {chooseHeroController.CurrentHeroStrategyData.ManaConponentData.MaxMana.ToString()}";

        //Skill 1
        //->Icon
        var skill = UidocumentsList[1].rootVisualElement.Q<VisualElement>("Skill1");
        skill.style.backgroundImage = new StyleBackground(chooseHeroController.CurrentHeroStrategyData.SkillStrategyData[0].Icon);
        //->Description
        var description = UidocumentsList[1].rootVisualElement.Q<Label>("SkillLabel1");
        description.text = chooseHeroController.CurrentHeroStrategyData.SkillStrategyData[0].Description;

        //Skill 2
        //->Icon
        skill = UidocumentsList[1].rootVisualElement.Q<VisualElement>("Skill2");
        skill.style.backgroundImage = new StyleBackground(chooseHeroController.CurrentHeroStrategyData.SkillStrategyData[1].Icon);
        //->Description
        description = UidocumentsList[1].rootVisualElement.Q<Label>("SkillLabel2");
        description.text = chooseHeroController.CurrentHeroStrategyData.SkillStrategyData[1].Description;

        //Skill 3
        //->Icon
        skill = UidocumentsList[1].rootVisualElement.Q<VisualElement>("Skill3");
        skill.style.backgroundImage = new StyleBackground(chooseHeroController.CurrentHeroStrategyData.SkillStrategyData[2].Icon);
        //->Description
        description = UidocumentsList[1].rootVisualElement.Q<Label>("SkillLabel3");
        description.text = chooseHeroController.CurrentHeroStrategyData.SkillStrategyData[2].Description;
    }
    void ChoseHeroAndStartGame() {
        scenesManager.StartGame();
    }
}
