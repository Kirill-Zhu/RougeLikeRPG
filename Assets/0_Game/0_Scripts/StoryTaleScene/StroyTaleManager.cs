using FMOD.Studio;
using FMODUnity;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Video;
using Zenject;
public class StroyTaleManager : MonoBehaviour {
    [SerializeField] EventReference soundTrack;
    [SerializeField] VideoPlayer player;
    [SerializeField] InputReader inputReader;
    
   [SerializeField] ScenesManager scenesManager;
    AudioManager audioManager;

    [Inject]
    void Construct(ScenesManager scenesManager, AudioManager audioManager) {
        this.scenesManager = scenesManager;
        this.audioManager = audioManager;   
    }
    private void Awake() {
        player.Play();
        audioManager.PlayMusic(soundTrack);
        inputReader.IsUsingSkill += SkipStory;
    }
    private void OnDisable() {
        inputReader.IsUsingSkill -= SkipStory;
    }
   public void SkipStory(bool _) {
        audioManager.StartPlayStandartMusic();
        scenesManager.LoadMainMenu();
    }
}
