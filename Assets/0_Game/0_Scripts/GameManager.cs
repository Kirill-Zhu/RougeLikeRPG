using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start() {
#if PLATFORM_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }
}
