using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelStatisticsUIMenu : MonoBehaviour {
    [Inject] Hero hero;
    [Inject] EventManager eventManager;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    [SerializeField] float lerpDuration = 2f;
    [SerializeField] LevelStatisticsData dataStats;
    [SerializeField] GameObject statisticsMenu;
    [SerializeField] TextMeshProUGUI physicsDamageTakenTextMesh;
    [SerializeField] TextMeshProUGUI fireDamageTakenTextMesh;
    [SerializeField] TextMeshProUGUI coldDamageTakenTextMesh;

    [SerializeField] TextMeshProUGUI physicsDamageDoneTextMesh;
    [SerializeField] TextMeshProUGUI fireDamageDoneTextMesh;
    [SerializeField] TextMeshProUGUI coldDamageDoneTextMesh;

    [SerializeField] TextMeshProUGUI totalTimeTextMesh;


    //Event bus
    EventBinding<OnPlayerDied> onPlayerDiedBinding;
    EventBinding<OnPlayerRessurect> onPlayeRessurectBinding;
    private void OnEnable() {
        onPlayerDiedBinding = new EventBinding<OnPlayerDied>(ShowStatsMenu);
        EventBus<OnPlayerDied>.Register(onPlayerDiedBinding);

        onPlayeRessurectBinding = new EventBinding<OnPlayerRessurect>(HideStatsMenu);
        EventBus<OnPlayerRessurect>.Register(onPlayeRessurectBinding);
    }
    private void OnDisable() {
        EventBus<OnPlayerDied>.Deregister(onPlayerDiedBinding);
        EventBus<OnPlayerRessurect>.Deregister(onPlayeRessurectBinding);
    }
    [ContextMenu("Show menu")]
    public async void ShowStatsMenu() {
        statisticsMenu.SetActive(true);
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
        statisticsMenu.SetActive(false);
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
