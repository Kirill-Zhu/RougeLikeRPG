using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ManaComponent : MonoBehaviour, IVisitable {

    public event Action<int> OnManaChange;
    public int mana = 100;
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
    public void ChangeMana(int value) {
        mana += value;
        OnManaChange?.Invoke(mana);

    }
}
