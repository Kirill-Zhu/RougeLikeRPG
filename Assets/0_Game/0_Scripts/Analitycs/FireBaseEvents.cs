using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
public class FireBaseEvents : MonoBehaviour
{
    [Inject]
    FireBaseManager fireBaseManager;
    [SerializeField] int levelIndex;


    private void OnEnable() {
        fireBaseManager.LevelStart(SceneManager.GetActiveScene().name);
        Debug.Log($"Enters level{levelIndex}");
    }
}
