using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [Inject, SerializeField] Hero Hero;
    [SerializeField] Vector3 offset;
    CinemachineCamera cinemachineCamera;
    private void Awake() {
        transform.position = Hero.transform.position + offset;
        transform.rotation = Quaternion.LookRotation(Hero.transform.position - transform.position);
        cinemachineCamera = GetComponent<CinemachineCamera>();
        cinemachineCamera.Follow = Hero.transform;
    }
}
