using UnityEngine;

public interface IEvent { }
public class OnPlayerAlive : IEvent {

    public Hero hero;
}
public class OnPlayerDied : IEvent {
    public Hero hero;
}
public class OnUpgradeItemInShop : IEvent {
    public readonly int Cost;
   public OnUpgradeItemInShop(int cost) {
        GameData.SpendCoins(cost);
    }
}
public class OnCoinCollected : IEvent {

    public int CoinsCollected;
    public int CurrentCoins => GameData.GetConins();
    public OnCoinCollected(int value) {
        CoinsCollected = value;
        GameData.AddCoins(value);
    }
}