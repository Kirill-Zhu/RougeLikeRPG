using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour {


    [SerializeField] public static int storyTaleSceneID = 1;
    [SerializeField] public static int mainMenuSceneID = 2;
    [SerializeField] public static int InGameUISceneID = 3;
    [SerializeField] public static int tutorialSceneID = 4;
    [SerializeField] public static int CatacombsEntry = 5;
    [SerializeField] public static int CatacombsSaints = 6;
    [SerializeField] public static int CatacombsCoffins = 7;    


    int prevLevelID = 0;

    private void Awake() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(storyTaleSceneID);

    }

    public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CatacombsEntry);
        UnityEngine.SceneManagement.SceneManager.LoadScene(InGameUISceneID, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        prevLevelID = CatacombsEntry;
    }
    public async UniTask LoadLevelAsync(int sceneID) {
        if (SceneManager.GetActiveScene().buildIndex == prevLevelID)
            await UnloadScene(prevLevelID);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
        UnityEngine.SceneManagement.SceneManager.LoadScene(InGameUISceneID, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    public void LoadMainMenu() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneID);
    }

    async UniTask UnloadScene(int scenId) {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenId);

        await UniTask.WaitUntil(() => asyncOperation.isDone);
    }

}
