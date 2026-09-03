using Cysharp.Threading.Tasks;
using UnityEngine;

public class GlobalVolumeManager : MonoBehaviour {


    GlobalVolumeController globlaVolumeController;
    [SerializeField] EventManager eventManager;

    public void SetEventManager(EventManager eventManager) {
        this.eventManager = eventManager;

    }
    public void SetUpGlobalVolumeController(GlobalVolumeController controller) {
        globlaVolumeController = controller;
    }
    [ContextMenu("Fade Out")]
    public async UniTask FadeOut() {
        Debug.Log("Fade Out");
        await globlaVolumeController.FadeOut(1);
    }
    [ContextMenu("Fade In")]
    public async UniTask FadeIn() {
        Debug.Log("Fade In");
        await globlaVolumeController.FadeIn(1);
    }

}
