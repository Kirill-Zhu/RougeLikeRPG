using UnityEngine;
using System.Reflection;
using System;

[CreateAssetMenu(menuName = "Strategy/Shop/ItemStrategy")]
public class ItemStrategy : ScriptableObject {

    public int Level = 0;
    [SerializeField] Item[] itemsArray;
    public void LevelUpItem() {
        if (Level + 1 < itemsArray.Length)
            Level++;
    }
    public Item NextItem() {
        try {
            return itemsArray[Level + 1];
        } catch {
            return itemsArray[Level];
        }
    }
    public Item CurrentItem() {
        return itemsArray[Level];
    }

   
}
