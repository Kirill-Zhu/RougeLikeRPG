using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;
public class DynamicTextUI : MonoBehaviour {

    [Inject] EventManager eventManager;
    [SerializeField] TextMeshProUGUI textMesh;
    float textLerpDuration = 1;
    int textLiveDuration = 3;
    EventBinding<OnSpawnBoss> onSpawnBossbinding;
    EventBinding<OnChangeWave> onChangeWaveBinding;
    Tween tween;
    private void Awake() {
        // eventManager.OnBossCreate.AddListener(ShowOnBossRaiseText);
    }

    private void OnEnable() {
        onSpawnBossbinding = new EventBinding<OnSpawnBoss>(ShowText);
        onChangeWaveBinding = new EventBinding<OnChangeWave>(ShowText);
        EventBus<OnSpawnBoss>.Register(onSpawnBossbinding);
        EventBus<OnChangeWave>.Register(onChangeWaveBinding);
    }
    private void OnDisable() {
        //Events
        EventBus<OnSpawnBoss>.Deregister(onSpawnBossbinding);
        EventBus<OnChangeWave>.Deregister(onChangeWaveBinding);
        //Tweens
        tween.Kill();
    }
    async void ShowText(OnSpawnBoss @event) {
        textMesh.gameObject.SetActive(true);
        textMesh.text = @event.Name;
        textMesh.rectTransform.localScale = Vector3.zero;
        tween = textMesh.rectTransform.DOScale(1, textLerpDuration).SetEase(Ease.InExpo);

        await UniTask.WaitForSeconds(textLiveDuration);
        HideText();
    }
    async void ShowText(OnChangeWave @event) {
        textMesh.gameObject.SetActive(true);
        textMesh.text = "Wave " + @event.wave;
        textMesh.rectTransform.localScale = Vector3.zero;
        tween = textMesh.rectTransform.DOScale(1, textLerpDuration).SetEase(Ease.InExpo);

        await UniTask.WaitForSeconds(textLiveDuration);
        HideText();
    }
    void HideText() {
        textMesh.rectTransform.DOScale(0, textLerpDuration).SetEase(Ease.InExpo);
    }
}
