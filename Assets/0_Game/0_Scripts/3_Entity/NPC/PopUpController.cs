using UnityEngine;
using DG.Tweening;
public class PopUpController : MonoBehaviour
{

    [SerializeField] GameObject exclamationMark;


    float animationDuration = 0.4f;
    Sequence sequence;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Hero>() == null) return;
        PopUpExcalamtionMark();
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Hero>() == null) return;
        PopDownExclamationMark();
    }
    public void PopUpExcalamtionMark() {
        Debug.Log("Pop up");
        sequence = DOTween.Sequence();
        sequence.Append(exclamationMark.transform.DOScale(1, animationDuration))
        .Join(exclamationMark.transform.DORotate(Vector3.up * 360, animationDuration, RotateMode.FastBeyond360)
        .SetEase(Ease.InOutBounce));
    }

    public void PopDownExclamationMark() {
        sequence = DOTween.Sequence();
        sequence.Append(exclamationMark.transform.DOScale(0, animationDuration))
       .Join(exclamationMark.transform.DORotate(Vector3.up * 360, animationDuration, RotateMode.FastBeyond360)
       .SetEase(Ease.InOutBounce));
    }
}
