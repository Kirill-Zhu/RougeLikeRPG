using UnityEngine;
using DG.Tweening;
public class CameraStartScreenContoller : MonoBehaviour
{
    [SerializeField] Vector3 finalRotation;
    [SerializeField] float lerttime;

    Tween tween;

    private void Start() {
        tween =  transform.DORotate(finalRotation, lerttime, RotateMode.Fast).SetEase(Ease.InExpo);
    }

    private void OnDestroy() {
        tween.Kill();
    }
}
