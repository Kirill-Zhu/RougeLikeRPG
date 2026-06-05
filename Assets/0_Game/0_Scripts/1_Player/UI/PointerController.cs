using UnityEngine;

public class PointerController : MonoBehaviour
{
    Vector3 target = Vector3.zero;
    [SerializeField] GameObject pointerGraphics;
    public void HandlePointToTarget() {

        transform.rotation = Quaternion.LookRotation(target.WithY(0)- transform.position.WithY(0), Vector3.up);
    }
    public void EnablePointer() {
        pointerGraphics.SetActive(true);
    }
    public void DisablePointer() {
        pointerGraphics.SetActive(false);
    }
    public void SetTarget(Vector3 target) {

        this.target = target;
    }
}
