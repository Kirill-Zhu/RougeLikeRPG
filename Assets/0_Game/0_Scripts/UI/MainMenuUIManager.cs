using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UIElements.Button;
public class MainMenuUIManager : MonoBehaviour {
    
    [SerializeField] List<UIDocument> UidocumentsList;
    [SerializeField] UIDocument catacombsUIDocument;
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
    const string WorldMap = "WorldMap";
    const string MapButton = "MapButton";
    const string StartTutorial = "StartTutorial";
    const string CatacombsMap = "CatacombsMap";
    const string Shop = "Shop";
    const string Language = "Language";
    const string Back = "Back";

    #region CatacombsRooms
    const string Entry = "Entry";
    const string Saints = "Saints";
    const string Coffins = "Coffins";
    #endregion
    int choosedSceneIndex = 5;

    #region DIZENJECT
    EventManager eventManager;
    LocalizationManager localizationManager;
    [Inject]
    void Construct(EventManager eventManager, LocalizationManager localizetionManager) {
        this.eventManager = eventManager;
        this.localizationManager = localizetionManager;
    }
    #endregion
    private void Awake() {
        OpenMainMenu();


        CloseChooseHeroMenu();
        CloseWorldMapMenu();
        CloseCatacombsMap();
    }

    private async void Start() {

        await UniTask.Delay(100);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    private void Update() {

    }
    void Initialize() {
        //Main menu
        var choseHeroButton = UidocumentsList[0].rootVisualElement.Q<Button>(ChooseHeroButton);
        var shopButton = UidocumentsList[0].rootVisualElement.Q<Button>(Shop);
        var languageButton = UidocumentsList[0].rootVisualElement.Q<Button>(Language);
        //Events
        choseHeroButton.clicked += OpenChooseHeroMenu;
        shopButton.clicked += OpenShop;
        languageButton.clicked += ToogleLanguage;
        //Map

    }
    public void OpenAuthentificationMenu() {

    }
    //Main menu
    void OpenMainMenu() {
        //-----Main Menu
        var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        var choseHeroButton = UidocumentsList[0].rootVisualElement.Q<Button>(ChooseHeroButton);
        var shopButton = UidocumentsList[0].rootVisualElement.Q<Button>(Shop);
        var languageButton = UidocumentsList[0].rootVisualElement.Q<Button>(Language);
        choseHeroButton.Focus();
        EventSystem.current.SetSelectedGameObject(gameObject);

        mainMenu.style.display = DisplayStyle.Flex;
        choseHeroButton.clicked += OpenChooseHeroMenu;
        shopButton.clicked += OpenShop;
        languageButton.clicked += ToogleLanguage;


    }
    void CloseMainMenu() {
        //-----Main Menu
        var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        var choseHeroButton = UidocumentsList[0].rootVisualElement.Q<Button>(ChooseHeroButton);
        var shopButton = UidocumentsList[0].rootVisualElement.Q<Button>(Shop);
        var languageButton = UidocumentsList[0].rootVisualElement.Q<Button>(Language);
        choseHeroButton.Focus();
        EventSystem.current.SetSelectedGameObject(gameObject);

        mainMenu.style.display = DisplayStyle.None;
        choseHeroButton.clicked -= OpenChooseHeroMenu;
        shopButton.clicked -= OpenShop;
        languageButton.clicked -= ToogleLanguage;
    }
    //Chose Hero menu
    void OpenChooseHeroMenu() {
        //Shop
        CloseShop();

        ////-----Main Menu
        //var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        //mainMenu.style.display = DisplayStyle.None;

        //-----Chose Hero Menu
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        var heroDesk = UidocumentsList[1].rootVisualElement.Q<VisualElement>(HeroDesk);

        var nextHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(NextHero);
        var prevHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(PrevHero);
        var mapButton = UidocumentsList[1].rootVisualElement.Q<Button>(MapButton);

        //Visability
        choseHeroMenu.style.display = DisplayStyle.Flex;
        startGameMenu.style.display = DisplayStyle.Flex;
        heroDesk.style.display = DisplayStyle.Flex;
        nextHeroButton.Focus();

        //Events
        nextHeroButton.clicked += ChooseNextHero;
        prevHeroButton.clicked += ChoosePrevHero;
        //mapButton.clicked += ChoseHeroAndStartGame;
        mapButton.clicked += OpenWorldMapMenu;


        ChangeHeroDescriptionDesk();
    }
    //World Map
    private void OpenWorldMapMenu() {
        CloseChooseHeroMenu();
        CloseMainMenu();
        var worldMap = UidocumentsList[2].rootVisualElement.Q<VisualElement>(WorldMap);
        var catacombs = UidocumentsList[2].rootVisualElement.Q<Button>(CatacombsMap);
        var startTutorial = UidocumentsList[2].rootVisualElement.Q<Button>(StartTutorial);
        var backButton = UidocumentsList[2].rootVisualElement.Q<Button>(Back);


        worldMap.style.display = DisplayStyle.Flex;

        //Events
        backButton.clicked += CloseWorldMapMenu;
        backButton.clicked += OpenChooseHeroMenu;

        catacombs.clicked += OpenCatacombsMap;
        startTutorial.clicked += StartTutorialScene;
    }

    void CloseWorldMapMenu() {
        var worldMap = UidocumentsList[2].rootVisualElement.Q<VisualElement>(WorldMap);
        var startGame = UidocumentsList[2].rootVisualElement.Q<Button>(CatacombsMap);
        var backButton = UidocumentsList[2].rootVisualElement.Q<Button>(Back);

        worldMap.style.display = DisplayStyle.None;

        //Events
        backButton.clicked -= CloseWorldMapMenu;
        backButton.clicked -= OpenChooseHeroMenu;

        startGame.clicked -= StartCatacombsEntry;
    }

    //------------

    #region Locations
    //Catacombs
    void OpenCatacombsMap() {
        CloseWorldMapMenu();

        var catacombsMap = catacombsUIDocument.rootVisualElement.Q<VisualElement>("CatacombsMap");
        catacombsMap.style.display = DisplayStyle.Flex;

        var backButton = catacombsUIDocument.rootVisualElement.Q<Button>(Back);

        //Rooms
        var entryRoom = catacombsUIDocument.rootVisualElement.Q<Button>(Entry);
        var saintsRoom = catacombsUIDocument.rootVisualElement.Q<Button>(Saints);
        var coffinsRoom = catacombsUIDocument.rootVisualElement.Q<Button>(Coffins);

        //Events

        backButton.clicked += CloseCatacombsMap;
        backButton.clicked += OpenWorldMapMenu;


        entryRoom.clicked += StartCatacombsEntry;
        int progress = GameData.GetProgressKey();
        Debug.Log($"Progress is {progress}");
        switch (progress) {
            case 1: {
                    saintsRoom.clicked += StartCatacombsSaints;
                    saintsRoom.style.unityBackgroundImageTintColor = Color.green;
                    break;
                }
            case 2: {
                    saintsRoom.clicked += StartCatacombsSaints;
                    saintsRoom.style.unityBackgroundImageTintColor = Color.green;

                    coffinsRoom.clicked += StartCatacombsCoffins;
                    coffinsRoom.style.unityBackgroundImageTintColor = Color.green;
                    break;

                }
        }
    }
    void CloseCatacombsMap() {
        var catacombsMap = catacombsUIDocument.rootVisualElement.Q<VisualElement>("CatacombsMap");
        catacombsMap.style.display = DisplayStyle.None;

        //Buttons
        var backButton = catacombsUIDocument.rootVisualElement.Q<Button>(Back);
        //Rooms
        var entryRoom = catacombsUIDocument.rootVisualElement.Q<Button>(Entry);
        var saintsRoom = catacombsUIDocument.rootVisualElement.Q<Button>(Saints);
        var coffinsRoom = catacombsUIDocument.rootVisualElement.Q<Button>(Coffins);
        //Events
        backButton.clicked -= CloseCatacombsMap;
        backButton.clicked -= OpenWorldMapMenu;

        entryRoom.clicked -= StartCatacombsEntry;
        int progress = GameData.GetProgressKey();
        switch (progress) {
            case 1: {
                    saintsRoom.clicked -= StartCatacombsSaints;
                    break;
                }
            case 2: {

                    saintsRoom.clicked -= StartCatacombsSaints;
                    coffinsRoom.clicked -= StartCatacombsCoffins;
                    break;

                }
        }
    }
    //--------------------
    void CloseChooseHeroMenu() {
        //-----Main Menu
        //var mainMenu = UidocumentsList[0].rootVisualElement.Q<VisualElement>(MainMenu);
        //mainMenu.style.display = DisplayStyle.Flex;

        //-----Chose Hero Menu
        var heroDesk = UidocumentsList[1].rootVisualElement.Q<VisualElement>(HeroDesk);
        var choseHeroMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(ChooseHeroMenu);
        var startGameMenu = UidocumentsList[1].rootVisualElement.Q<VisualElement>(StartGameMenu);
        var nextHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(NextHero);
        var prevHeroButton = UidocumentsList[1].rootVisualElement.Q<Button>(PrevHero);
        var mapButton = UidocumentsList[1].rootVisualElement.Q<Button>(MapButton);


        //Visability
        choseHeroMenu.style.display = DisplayStyle.None;
        startGameMenu.style.display = DisplayStyle.None;
        heroDesk.style.display = DisplayStyle.None;

        //Events
        nextHeroButton.clicked -= ChooseNextHero;
        prevHeroButton.clicked -= ChoosePrevHero;
        mapButton.clicked -= OpenWorldMapMenu;

        //Focus

    }
    #endregion

    //Shop
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
        heroStory.text = chooseHeroController.CurrentHeroStrategyData.GetStory();

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
    void StartTutorialScene() {
        eventManager.StartTutorial();
    }
    void StartCatacombsEntry() {
        eventManager.StartCatacombsEntry();
    }
    void StartCatacombsSaints() {
        eventManager.StartCatacombsSaints();
    }
    void StartCatacombsCoffins() {
        eventManager.StartCatacombsCoffins();
    }

    #region Localization
    void ToogleLanguage() {
        localizationManager.ToogleLanguage();
    }
    #endregion
}

