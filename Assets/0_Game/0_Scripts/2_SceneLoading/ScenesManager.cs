using UnityEngine;


public class ScenesManager : MonoBehaviour
{
    [SerializeField] int mainMenuSceneID = 1;
    [SerializeField] int InGameUISceneID = 2;
    [SerializeField] int TestLevleSceneID = 3;


    private void Awake() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneID);
    }

    public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(TestLevleSceneID);
        UnityEngine.SceneManagement.SceneManager.LoadScene(InGameUISceneID, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    public void LoadScene(int sceneID) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
        UnityEngine.SceneManagement.SceneManager.LoadScene(InGameUISceneID, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
