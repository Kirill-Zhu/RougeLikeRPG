using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
public class SkillButtonsController : MonoBehaviour
{
    [SerializeField] Button westButton;
    [SerializeField] Button northButton;
    [SerializeField] Button eastButton;

    Gamepad virtualGamepad;
    private void Awake() {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
    }
    private void OnEnable() {

        westButton.onClick.AddListener(WestButton);
        northButton.onClick.AddListener(NorthButton);
        eastButton.onClick.AddListener(EastButton);
    }

    void WestButton() {
        InputSystem.QueueStateEvent(virtualGamepad, new GamepadState(GamepadButton.West));
        var releaseState = new GamepadState();
        InputSystem.QueueStateEvent(virtualGamepad, releaseState);

    }
    void NorthButton() {
        InputSystem.QueueStateEvent(virtualGamepad, new GamepadState(GamepadButton.North));
        var releaseState = new GamepadState();
        InputSystem.QueueStateEvent(virtualGamepad, releaseState);
    }

    void EastButton() {
        InputSystem.QueueStateEvent(virtualGamepad, new GamepadState(GamepadButton.East));
        var releaseState = new GamepadState();
        InputSystem.QueueStateEvent(virtualGamepad, releaseState);
    }
}
