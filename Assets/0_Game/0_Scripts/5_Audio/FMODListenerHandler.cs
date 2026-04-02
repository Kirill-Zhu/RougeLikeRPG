using FMODUnity;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(StudioListener))]
public class FMODListenerHandler : MonoBehaviour {
    StudioListener studioListener;
    [Inject] Hero hero;
    private void Awake() {
        studioListener = GetComponent<StudioListener>();
        studioListener.AttenuationObject = hero.gameObject;
    }
}
