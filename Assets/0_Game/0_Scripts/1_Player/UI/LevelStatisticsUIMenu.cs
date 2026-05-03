using TMPro;
using UnityEngine;
using Zenject;

public class LevelStatisticsUIMenu : MonoBehaviour {
    [Inject] Hero hero;
    [Inject] EventManager eventManager;
    [SerializeField] LevelStatisticsData dataStats;
    [SerializeField] GameObject statisticsMenu;
    [SerializeField] TextMeshProUGUI physicsDamageTakenTextMesh;
    [SerializeField] TextMeshProUGUI fireDamageTakenTextMesh;
    [SerializeField] TextMeshProUGUI coldDamageTakenTextMesh;

    [SerializeField] TextMeshProUGUI physicsDamageDoneTextMesh;
    [SerializeField] TextMeshProUGUI fireDamageDoneTextMesh;
    [SerializeField] TextMeshProUGUI coldDamageDoneTextMesh;

    [SerializeField] TextMeshProUGUI totalTimeTextMesh;

    private void Awake() {
        hero.OnDie.AddListener(() => ShowStatsMenu());
    }
    public void ShowStatsMenu() {
        statisticsMenu.SetActive(true);
        UpdateStats();
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
        eventManager.OnLoadMainMenu.Invoke();
    }
}
