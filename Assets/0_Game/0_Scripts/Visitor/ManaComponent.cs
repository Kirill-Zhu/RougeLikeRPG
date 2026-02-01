using UnityEngine;

public class ManaComponent : MonoBehaviour, IVisitable {

    public int mana = 100;
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
    public void ChangeMana(int value) {
        mana += value;
    }
}
