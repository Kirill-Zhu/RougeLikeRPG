using UnityEngine;
[CreateAssetMenu(menuName = "Visitor/Coin", fileName = "New Coin")]
public class Coin : PowerUp {
    [SerializeField] int coinValue = 1;
    public void Visit(CoinsComponent coinsComponent) {
        Debug.Log("Visit coins");
        coinsComponent.CollectCoin(coinValue);
    }
}
