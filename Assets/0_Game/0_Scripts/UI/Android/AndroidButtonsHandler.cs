using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class AndroidButtonsHandler : MonoBehaviour,IPointerDownHandler, IPointerUpHandler {
    public GamepadButton button;

    Gamepad virtualGamepad;

    private void Awake() {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
    }
    public void OnPointerDown(PointerEventData eventData) {
        InputSystem.QueueStateEvent(virtualGamepad, new GamepadState(button));
       
    }

    public void OnPointerUp(PointerEventData eventData) {
        var releaseState = new GamepadState();
        InputSystem.QueueStateEvent(virtualGamepad, releaseState);
    }
}
