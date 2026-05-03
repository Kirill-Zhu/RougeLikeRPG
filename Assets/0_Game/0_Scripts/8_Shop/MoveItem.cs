using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Shop/MoveItem")]
public class MoveItem: Item {
    public int AddMoveSpeed = 1;

    public void Visit(SimpleCahracterController characterController) { 
        characterController.AddItem(this);
    }
}
