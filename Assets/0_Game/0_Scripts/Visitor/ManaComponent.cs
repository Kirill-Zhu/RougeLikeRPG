using System;
using UnityEngine;

public class ManaComponent : MonoBehaviour, IVisitable {

    public event Action<int> OnGetCurrentMana;
    public int MaxMana = 100;
    public int CurrentMana = 100;
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }

    public void ChangeMana(int value) {
        CurrentMana += value;
        OnGetCurrentMana?.Invoke(CurrentMana);

    }
}
