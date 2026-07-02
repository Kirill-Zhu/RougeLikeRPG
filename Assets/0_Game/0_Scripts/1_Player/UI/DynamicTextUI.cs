using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;
public class DynamicTextUI : MonoBehaviour {

    [Inject] EventManager eventManager;
    [SerializeField] GameObject textGameObject;
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
        textGameObject.gameObject.SetActive(true);
        textMesh.text = @event.Name;
        var rectTransform = textGameObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        tween = rectTransform.DOScale(1, textLerpDuration).SetEase(Ease.InExpo);

        await UniTask.WaitForSeconds(textLiveDuration);
        HideText();
    }
    async void ShowText(OnChangeWave @event) {
        textGameObject.gameObject.SetActive(true);
        textMesh.text = "Wave " + @event.wave.ToString();
        
        var rectTransform = textGameObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        tween = rectTransform.DOScale(1, textLerpDuration).SetEase(Ease.InExpo);

        await UniTask.WaitForSeconds(textLiveDuration);
        HideText();
    }
    void HideText() {
        textGameObject.GetComponent<RectTransform>().DOScale(0, textLerpDuration).SetEase(Ease.InExpo);
    }
}
