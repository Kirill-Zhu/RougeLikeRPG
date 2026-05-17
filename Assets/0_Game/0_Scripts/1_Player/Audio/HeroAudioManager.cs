using FMODUnity;
using UnityEngine;

public class HeroAudioManager : MonoBehaviour
{
   public void PlayOneShot(EventReference eventReference, Vector3 position) {
        RuntimeManager.PlayOneShot(eventReference, position);
    }
}
