using UnityEngine;
using Zenject;

public class MinimapCameraController : MonoBehaviour
{
    [Inject] Hero hero;
    [SerializeField] Vector3 offset;
    [SerializeField] Transform cam;


    private void Awake() {
        cam.transform.position = hero.transform.position + offset;
    }

    private void LateUpdate() {
        cam.transform.position = hero.transform.position + offset;
    }
}
