using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class LocalAudioLibrary : MonoBehaviour
{
   [SerializeField] EventReference evnetReference;
    EventInstance music;

    private void Awake() {
        PlayeMusic();
    }

    void PlayeMusic() {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(evnetReference);
        music.start();
    }
}
