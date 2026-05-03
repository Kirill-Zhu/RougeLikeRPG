using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Visitor/Coin", fileName = "New Coin")]
public class CoinsComponent : MonoBehaviour, IVisitable {
    Hero hero;
    EventBinding<OnCoinCollected> onCoinCollected;
    private void OnEnable() {
        onCoinCollected = new EventBinding<OnCoinCollected>(OnCollectCoin);
        EventBus<OnCoinCollected>.Register(onCoinCollected);
    }

    private void OnDestroy() {
        EventBus<OnCoinCollected>.Deregister(onCoinCollected);  
    }
    public void Initialaize(Hero hero) {
        this.hero = hero;   
    }
    public void CollectCoin(int value) {
        Debug.Log($"{GetType().Name} is Collect Conin");
        EventBus<OnCoinCollected>.Raise(new OnCoinCollected(value) { CoinsCollected = value,  });
    }

    void OnCollectCoin(OnCoinCollected onCoinCollected) {


        Debug.Log($"{GetType().Name} Get Collect Coin Call");
    }

    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
}