using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AndoridArrowsManager : MonoBehaviour
{
    [SerializeField] List<AndroidArrowsHandler> managers;

    public  Vector2 direction;
    Gamepad virtualGamepad;
    private void Awake() {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
    }
    private void Update() {
        direction = Vector2.zero;
        foreach (var manager in managers) {
            direction += manager.Direction;
        }

        InputSystem.QueueDeltaStateEvent(virtualGamepad.leftStick, direction);
    }
}
