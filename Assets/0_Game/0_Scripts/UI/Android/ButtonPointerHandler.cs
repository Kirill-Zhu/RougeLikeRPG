using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;

public class ButtonPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Gamepad virtualGamepad;
    private void Awake() {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Pointer Enter");
        InputSystem.QueueStateEvent(virtualGamepad, new GamepadState(GamepadButton.South));
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Pointer Exit");
        var releaseState = new GamepadState();
        InputSystem.QueueStateEvent(virtualGamepad, releaseState);
    }

}
