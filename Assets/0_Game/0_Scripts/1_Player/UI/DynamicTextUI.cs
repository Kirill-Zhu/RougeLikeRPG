using BossEntity;
using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;
public class DynamicTextUI : MonoBehaviour {

    [Inject] EventManager eventManager;
    [SerializeField] TextMeshProUGUI textMesh;
    EventBinding<OnSpawnBoss> onSpawnBoss;
    Tween tween;
    private void Awake() {
       // eventManager.OnBossCreate.AddListener(ShowOnBossRaiseText);
    }

    private void OnEnable() {
        onSpawnBoss = new EventBinding<OnSpawnBoss>(ShowText);
        EventBus<OnSpawnBoss>.Register(onSpawnBoss);
    }
    private void OnDisable() {
        //Events
        EventBus<OnSpawnBoss>.Deregister(onSpawnBoss);
        //Tweens
        tween.Kill();
    }
    void ShowText(OnSpawnBoss @event) {
        textMesh.gameObject.SetActive(true);
        textMesh.text = @event.Name;
        textMesh.rectTransform.localScale = Vector3.zero;
        tween = textMesh.rectTransform.DOScale(1, 2).SetEase(Ease.InExpo);
    }
    void HideText() {
        textMesh.rectTransform.DOScale(0, 2).SetEase(Ease.InExpo);
    }
}
