using UnityEngine;
using UnityEngine.Events;
public class ExpComponent : MonoBehaviour, IVisitable {
    UnityEvent<int> OnLvlUp;
    UnityEvent<int, int> OnGetExp;
    public int level = 1;
    public int currentExp = 0;
    public int MaxExp = 4;
    public int OverallExp;

    public void Initialize(UnityEvent<int> OnLevelUp, UnityEvent<int, int> OnGetExp) {
        OnLvlUp = OnLevelUp;
        this.OnGetExp = OnGetExp;
    }
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }

    public void GetExp(int value) {
        currentExp += value;
        OverallExp += value;
        OnGetExp.Invoke(value, currentExp);
        if (currentExp >= MaxExp)
            LevelUp();
    }

    void LevelUp() {
        level++;
        currentExp = 0;
        MaxExp = (int)Mathf.Pow((int)4, level);
        OnLvlUp.Invoke(MaxExp);
    }
}
