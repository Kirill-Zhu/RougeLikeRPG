using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelStatisticsUIMenu : MonoBehaviour {
    [Inject] Hero hero;
    [Inject] EventManager eventManager;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    [SerializeField] float lerpDuration = 2f;
    [SerializeField] LevelStatisticsData dataStats;
    [SerializeField] GameObject statisticsMenu;
    [Header("Buttons")]
    [SerializeField] Button toMenuButton;
    [SerializeField] Button reviveButton;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI topText;
    readonly string endLevelText = "you win";
    readonly string dieText = "You have been slayed";
    [SerializeField] TextMeshProUGUI physicsDamageTakenTextMesh;
    [SerializeField] TextMeshProUGUI fireDamageTakenTextMesh;
    [SerializeField] TextMeshProUGUI coldDamageTakenTextMesh;

    [SerializeField] TextMeshProUGUI physicsDamageDoneTextMesh;
    [SerializeField] TextMeshProUGUI fireDamageDoneTextMesh;
    [SerializeField] TextMeshProUGUI coldDamageDoneTextMesh;

    [SerializeField] TextMeshProUGUI totalTimeTextMesh;


    //Event bus
    EventBinding<OnPlayerEndLevel> onPlayerEndLevelBinding;
    EventBinding<OnPlayerDied> onPlayerDiedBinding;
    EventBinding<OnPlayerRessurect> onPlayeRessurectBinding;
    private void OnEnable() {
        onPlayerEndLevelBinding = new EventBinding<OnPlayerEndLevel>(ShowEndLevelMenu);
        EventBus<OnPlayerEndLevel>.Register(onPlayerEndLevelBinding);

        onPlayerDiedBinding = new EventBinding<OnPlayerDied>(ShowDieMenu);
        EventBus<OnPlayerDied>.Register(onPlayerDiedBinding);

        onPlayeRessurectBinding = new EventBinding<OnPlayerRessurect>(HideStatsMenu);
        EventBus<OnPlayerRessurect>.Register(onPlayeRessurectBinding);
    }
    private void OnDisable() {
        EventBus<OnPlayerEndLevel>.Deregister(onPlayerEndLevelBinding);
        EventBus<OnPlayerDied>.Deregister(onPlayerDiedBinding);
        EventBus<OnPlayerRessurect>.Deregister(onPlayeRessurectBinding);
    }
    [ContextMenu("Show menu")]
    public async void ShowDieMenu() {
        statisticsMenu.SetActive(true);
        topText.text = dieText;
        reviveButton.gameObject.SetActive(true);
        var rectTransform = statisticsMenu.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos;
        UpdateStats();
        await rectTransform.DOAnchorPos(endPos, lerpDuration).SetEase(Ease.InOutBack).SetUpdate(true).ToUniTask();
    }
    public async void ShowEndLevelMenu() {
        statisticsMenu.SetActive(true);
        topText.text = endLevelText;
        reviveButton.gameObject.SetActive(false);
        var rectTransform = statisticsMenu.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos;
        UpdateStats();
        await rectTransform.DOAnchorPos(endPos, lerpDuration).SetEase(Ease.InOutBack).SetUpdate(true).ToUniTask();
    }

    [ContextMenu("Hide menu")]
    public async void HideStatsMenu() {
        var rectTransform = statisticsMenu.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = endPos;
        await rectTransform.DOAnchorPos(startPos, lerpDuration).SetEase(Ease.InOutBack).SetUpdate(true).ToUniTask();

        try {
            statisticsMenu.SetActive(false);
        }
        catch
        {
            Debug.LogWarning("Statistics menu has been destroyed");
        }
    }

    [ContextMenu("Update stats menu")]
    public void UpdateStats() {
        physicsDamageTakenTextMesh.text = "Physics :" + dataStats.PhysicsDamageTake;
        fireDamageTakenTextMesh.text = "Fire :" + dataStats.FireDamageTake;
        coldDamageTakenTextMesh.text = "Cold :" + dataStats.ColdDamageTake;

        physicsDamageDoneTextMesh.text = "Physics :" + dataStats.PhysicsDamageDone;
        fireDamageDoneTextMesh.text = "Fire :" + dataStats.FireDamageDone;
        coldDamageDoneTextMesh.text = "Cold :" + dataStats.ColdDamageDone;

        //Color
        physicsDamageTakenTextMesh.color = new PhysicsDamageType(0);
        physicsDamageDoneTextMesh.color = new PhysicsDamageType(0);

        fireDamageTakenTextMesh.color = new FireDamageType(0);
        fireDamageDoneTextMesh.color = new FireDamageType(0);

        coldDamageTakenTextMesh.color = new ColdDamageType(0);
        coldDamageDoneTextMesh.color = new ColdDamageType(0);
    }

    public void Confim() {
        eventManager.LoadMainMenu();
    }
}
