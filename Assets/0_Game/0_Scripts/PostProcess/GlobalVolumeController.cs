using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using VolFx;
using Zenject;

public class GlobalVolumeController : MonoBehaviour
{
    [Inject] GlobalVolumeManager volumeManager;
    [SerializeField] Volume volume;
    AsciiVol asciVolume;
    float fadeOutValue =0f;
    float fadeInValue = 1.0f;
    private void OnEnable() {
        volumeManager.SetUpGlobalVolumeController(this);
    }
    public async UniTask FadeOut(float duration) {
        volume.profile.TryGet(out asciVolume);
        asciVolume.m_Scale.value = fadeInValue;
        await DOTween.To(() => asciVolume.m_Scale.value, x => asciVolume.m_Scale.value = x, fadeOutValue, duration).AsyncWaitForCompletion();
    }
    public async UniTask FadeIn(float duration) {
        volume.profile.TryGet(out asciVolume);
        asciVolume.m_Scale.value = fadeOutValue;
        await DOTween.To(() => asciVolume.m_Scale.value, x => asciVolume.m_Scale.value = x, fadeInValue, duration).AsyncWaitForCompletion();
    }
   
}
