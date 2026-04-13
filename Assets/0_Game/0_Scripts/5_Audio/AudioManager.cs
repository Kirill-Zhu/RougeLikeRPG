using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Zenject;
using BossEntity;

public class AudioManager : MonoBehaviour
{
    EventInstance music;
    [SerializeField] EventReference reference;
    [SerializeField] EventReference bossFightMusic;
    
    //Dependencies
   [SerializeField] EventManager eventManager;
    private void Start() {
        try {
            Play();
        }
        catch { 
        
        }
    }
    
    public void Play() {
        music = RuntimeManager.CreateInstance(reference);
        music.start();
        //RuntimeManager.PlayOneShot(reference, new Vector3(38,0,66));
    }
    public void PlayOneShot(EventReference reference, Vector3 pos) {
        RuntimeManager.PlayOneShot(reference, pos);
    }
 
    public void StartBossFightMusic() {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(bossFightMusic);
        music.start();
    }
    public void StartBossFightMusic(Boss boss) {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.release();
        music = RuntimeManager.CreateInstance(bossFightMusic);
        music.start();
    }

    public void SetEventManager(EventManager eventManager) { 
       this.eventManager = eventManager;
        eventManager.OnBossCreate.AddListener(StartBossFightMusic);
    }
}
