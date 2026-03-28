using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    void Update() {
        // Detect if the current gamepad was updated this frame
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame) {
            HideCursor();
        }

        // Detect if the mouse was moved or clicked
        if (Mouse.current != null && (Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f || Mouse.current.leftButton.wasPressedThisFrame)) {
            ShowCursor();
        }
    }

    void HideCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Keeps cursor centered to avoid clicking off-screen
    }

    void ShowCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
