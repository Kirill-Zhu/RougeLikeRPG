using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class AndroidArrowsHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public Arrow ArrowKey;
    public Vector2 Direction;

    public void OnPointerDown(PointerEventData eventData) {
        Direction = GetDirection();
    }

    public void OnPointerUp(PointerEventData eventData) {
      Direction = Vector2.zero;    
    }
    Vector2 GetDirection() => ArrowKey switch {
        Arrow.Left => Vector2.left,
        Arrow.Right => Vector2.right,   
        Arrow.Up => Vector2.up,
        Arrow.Down => Vector2.down,
    };

}
public enum Arrow {
    Left,
    Right,
    Up,
    Down
}