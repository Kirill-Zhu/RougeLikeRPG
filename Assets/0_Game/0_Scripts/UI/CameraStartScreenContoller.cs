using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
public class CameraStartScreenContoller : MonoBehaviour {
    [SerializeField] Vector3 finalRotation;
    [SerializeField] float lerttime;

    Tween tween;

    private void OnEnable() { 
        tween =  transform.DORotate(finalRotation, lerttime, RotateMode.Fast).SetEase(Ease.InExpo);
    }
    private void OnDestroy() {
        tween.Kill();
    }
}
