using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference reference;

    [ContextMenu("Play Test")]

    private void Awake() {
        PlayOneShot();
    }
    public void PlayOneShot() {
        RuntimeManager.PlayOneShot(reference, new Vector3(38,0,66));
    }
    public void PlayOneShot(EventReference reference, Vector3 pos) {
        RuntimeManager.PlayOneShot(reference, pos);
    }
}
