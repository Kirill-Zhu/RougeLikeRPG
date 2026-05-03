using TMPro;
using UnityEngine;


public class ShopManager : MonoBehaviour {

    [SerializeField] GameObject shopDesk;
    [SerializeField] ItemStrategy[] strategyArray;
    [SerializeField] GameObject ItemCardPrefab;
    [SerializeField] RectTransform cardOrigin;
    ItemCard[] cardArray;
    Slot<ItemStrategy>[] slotsArray;
    [Header("Coins")]
    [SerializeField] TextMeshProUGUI coinsTextMesh;


    //Event Bus
    EventBinding<OnUpgradeItemInShop> onUpgradeItemInShop;
    private void OnEnable() {
        onUpgradeItemInShop = new EventBinding<OnUpgradeItemInShop>(SpendCoins);
        EventBus<OnUpgradeItemInShop>.Register(onUpgradeItemInShop);
    }
    private void OnDisable() {
        EventBus<OnUpgradeItemInShop>.Deregister(onUpgradeItemInShop);
    }

    private void Awake() {

        Debug.Log($"Coins now {GameData.GetConins()}");

        slotsArray = new Slot<ItemStrategy>[strategyArray.Length];
        cardArray = new ItemCard[strategyArray.Length];

        //Instantiate cards
        for (int i = 0; i < cardArray.Length; i++) {
            var obj = Instantiate(ItemCardPrefab, cardOrigin);
            obj.TryGetComponent<ItemCard>(out var card);
            cardArray[i] = card;
        }


        //Create slots
        for (int i = 0; i < slotsArray.Length; i++) {
            ItemStrategy strategy = Instantiate(strategyArray[i]);
            slotsArray[i] = new Slot<ItemStrategy>(strategyArray[i], cardArray[i]);
        }

        //Coins UI
        coinsTextMesh.text = GameData.GetConins().ToString();
    }

    public void OpenShop() {
        shopDesk.gameObject.SetActive(true);
    }

    public void CloseShop() {
        shopDesk.gameObject.SetActive(false);
    }
    [ContextMenu("Show Coins Count")]
    public void GetCoins() {
        Debug.Log($"Coins count is  {GameData.GetConins()}");
    }

    [ContextMenu("Add Coin")]
    public void AddCoins() {
        GameData.AddCoins(111);
    }
    void SpendCoins(OnUpgradeItemInShop @event) {

        Debug.Log("Spend coins in shop");
        GameData.SpendCoins(@event.Cost);
        foreach (var slot in slotsArray)
            slot.Refresh();


        //Coins UI
        coinsTextMesh.text = GameData.GetConins().ToString();
    }

   
    public Item[] GetItems() {
        Item[] items = new Item[slotsArray.Length];
        for (int i = 0; i < slotsArray.Length; i++) {
            items[i] = slotsArray[i].ItemStrategy.CurrentItem();
        }
        return items;
    }
    class Slot<T> where T : ItemStrategy {

        public readonly T ItemStrategy;
        ItemCard card;
        public Slot(T item, ItemCard card) {
            this.ItemStrategy = item;
            this.card = card;
            card.BuyButton.onClick.AddListener(BuyUpgrade);

            Refresh();
        }

        public void Refresh() {

            string description = ItemStrategy.NextItem() != ItemStrategy.CurrentItem() ? ItemStrategy.NextItem().Description : "Max";
            card.Initialize(description, ItemStrategy.NextItem().Label, ItemStrategy.NextItem().Cost);


            if (GameData.GetConins() >= ItemStrategy.NextItem().Cost && ItemStrategy.NextItem() != ItemStrategy.CurrentItem()) {
                card.BuyButton.enabled = true;
            } else {
                card.BuyButton.enabled = false;
            }

            //Coins UI
            
        }

        public void BuyUpgrade() {
            //Event Bus
            ItemStrategy.LevelUpItem();
            EventBus<OnUpgradeItemInShop>.Raise(new OnUpgradeItemInShop(ItemStrategy.CurrentItem().Cost));

        }
    }
}
public interface IItem {

}
