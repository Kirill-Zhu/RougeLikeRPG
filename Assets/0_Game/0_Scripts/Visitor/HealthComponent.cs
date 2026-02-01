using UnityEngine;
public class HealthComponent : MonoBehaviour, IVisitable {

    public int health = 100;
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
    public void ChangeHealth(int value) {
        health += value;
    }
}
