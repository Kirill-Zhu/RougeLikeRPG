using UnityEngine;
using DG.Tweening;
public class TextLoadSreenController : MonoBehaviour
{
    Tween tween;
    private void Start() {
        tween = transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy() {
        tween.Kill();
    }
}
