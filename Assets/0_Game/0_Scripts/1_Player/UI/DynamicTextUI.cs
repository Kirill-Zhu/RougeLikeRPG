using BossEntity;
using TMPro;
using UnityEngine;
using Zenject;
public class DynamicTextUI : MonoBehaviour {

    [Inject] EventManager eventManager;
    [SerializeField] TextMeshProUGUI textMesh;

    private void Awake() {
        eventManager.OnBossCreate.AddListener(ShowOnBossRaiseText);
    }

    public void ShowOnBossRaiseText(Boss boss) {
        string text = $"{boss.gameObject.name} is coming";
        ShowText(text);
    }
    void ShowText(string text) {
        textMesh.gameObject.SetActive(true);
        textMesh.text = text;
    }
}
