using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

public interface IInputReader {
    Vector3 Direction { get; }
    void EnablePlayerActions();
}
[CreateAssetMenu(menuName = "InputSystem", fileName = "InputReader")]
public class InputReader : ScriptableObject, IPlayerActions, IInputReader {
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<bool> Jump = delegate { };
    public event UnityAction<int> UseSkill = delegate { };
    public event UnityAction<bool> IsUsingSkill = delegate { };
    public bool IsMeleAttack = false;
    public PlayerInput InputActions;
    public Vector3 Direction => InputActions.Player.Move.ReadValue<Vector2>();

    InputAction.CallbackContext context = new InputAction.CallbackContext();

    IDisposable meleAttack;

    public void EnablePlayerActions() {
        if (InputActions == null) {
            InputActions = new PlayerInput();
            InputActions.Player.SetCallbacks(this);
        }
        InputActions.Enable();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context) {

    }

    public void OnButtonWest(InputAction.CallbackContext context) {

        if (context.phase == InputActionPhase.Started)
            IsUsingSkill.Invoke(true);


        if (context.phase == InputActionPhase.Started)
            UseSkill(1);
        //Debug.Log($"Mele attack stattus is{MeleAttack}");
    }

    public void OnButtonNorth(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started)
            IsUsingSkill.Invoke(true);
        UseSkill.Invoke(0);
    }

    public void OnButtonEast(InputAction.CallbackContext context) {

    }

    public void OnButtonSouth(InputAction.CallbackContext context) {
        this.context = context;
        switch (context.phase) {
            case InputActionPhase.Started:
                Jump.Invoke(true);
                break;

            case InputActionPhase.Canceled:
                Jump.Invoke(false);
                break;

        }
    }

    public void OnPrevious(InputAction.CallbackContext context) {

    }

    public void OnNext(InputAction.CallbackContext context) {

    }

    public void OnSprint(InputAction.CallbackContext context) {

    }
    private void OnDestroy() {
        meleAttack.Dispose();
    }
}