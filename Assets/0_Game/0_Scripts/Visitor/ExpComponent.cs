using UnityEngine;
using UnityEngine.Events;
public class ExpComponent : MonoBehaviour, IVisitable {
    public UnityEvent OnLelveUp;
    public int level = 1;
    public int currentExp = 0;
    public int OverallExp;

    public void Initialize(UnityEvent @event) {
        OnLelveUp = @event;
    }
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }

    public void GetExp(int value) {
        currentExp += value;
        OverallExp += value;
        if (currentExp >= level * 5)
            LevelUp();
    }

    void LevelUp() {
        OnLelveUp.Invoke();
        level++;
        currentExp = 0;
    }
}
