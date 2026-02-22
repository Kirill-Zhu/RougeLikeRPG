using UnityEngine;

[CreateAssetMenu(menuName = "Visitor/Exp", fileName = "New Exp" )]
public class EXPPickUp : PowerUp
{
    [SerializeField] int expValue = 1;
    public void Visit(ExpComponent exp) {
        exp.GetExp(expValue);
    }
}
