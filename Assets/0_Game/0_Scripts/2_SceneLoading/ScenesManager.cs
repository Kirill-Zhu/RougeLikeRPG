using UnityEngine;


public class ScenesManager : MonoBehaviour
{
    [SerializeField] int TestLevleSceneID = 0;
    [SerializeField] int InGameUISceneID = 1;


    private void Awake() {
        //Test
        UnityEngine.SceneManagement.SceneManager.LoadScene(TestLevleSceneID);
        UnityEngine.SceneManagement.SceneManager.LoadScene(InGameUISceneID, UnityEngine.SceneManagement.LoadSceneMode.Additive);


    }
}
