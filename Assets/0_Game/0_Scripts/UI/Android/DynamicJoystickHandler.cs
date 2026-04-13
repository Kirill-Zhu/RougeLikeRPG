using UnityEngine;
using Zenject;

[RequireComponent(typeof(DynamicJoystick))]
public class DynamicJoystickHandler : MonoBehaviour {
    [Inject] Hero hero;
    DynamicJoystick joystick;

    private void Awake() {
        joystick = GetComponent<DynamicJoystick>();
        hero.SetJoystick(joystick);
    }
}
