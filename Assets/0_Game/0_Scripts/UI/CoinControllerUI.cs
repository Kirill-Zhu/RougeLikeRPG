using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class CoinControllerUI : MonoBehaviour, InGameUI
{
    [SerializeField] TextMeshProUGUI textMexhCollectedCoin;
    [SerializeField] TextMeshProUGUI textMexhCurrentCoins;
    [SerializeField] Image coinImage;
    Hero hero;

    EventBinding<OnCoinCollected> onCoinCollected;
    EventBinding<OnUpgradeItemInShop> onUpgradeItemInShop;
    private void OnEnable() {
        onCoinCollected = new EventBinding<OnCoinCollected>(OnCollectCoin);
        EventBus<OnCoinCollected>.Register(onCoinCollected);

        onUpgradeItemInShop = new EventBinding<OnUpgradeItemInShop>(OnUpgradeItemInShop);
        EventBus<OnUpgradeItemInShop>.Register(onUpgradeItemInShop);
    }
    private void OnDisable() {
        EventBus<OnCoinCollected>.Deregister(onCoinCollected);
        EventBus<OnUpgradeItemInShop>.Deregister(onUpgradeItemInShop);
    }
    public void Initialaize(Hero hero) {
        this.hero = hero;
    }
    #region InGameUI
    [Header("Sho Hide Animaiton settings")]
    [SerializeField] Vector2 showPos;
    [SerializeField] Vector2 hidePos;
    [SerializeField] RectTransform transformToMove;
    Sequence sequence;
    [ContextMenu("Show UI")]
    public void ShowUI() {
        if (sequence != null)
            sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(transformToMove.DOAnchorPos(showPos, InGameUI.duration)).SetEase(Ease.InFlash);

    }
    [ContextMenu("hide Ui")]
    public void HideUI() {
        if (sequence != null)
            sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(transformToMove.DOAnchorPos(hidePos, InGameUI.duration)).SetEase(Ease.InFlash);
    }
    #endregion
    void OnCollectCoin(OnCoinCollected @event) {
        textMexhCollectedCoin.text = @event.CoinsCollected.ToString();
        textMexhCurrentCoins.text = GameData.GetConins().ToString();
        Debug.Log($"{GetType().Name} Get OnCoinCollectd Call");
    }

    void OnUpgradeItemInShop(OnUpgradeItemInShop @event) {
        textMexhCollectedCoin.text = @event.Cost.ToString();
        textMexhCurrentCoins.text = GameData.GetConins().ToString();
        Debug.Log($"{GetType().Name} Get OnCoinCollectd Call");
    }
}
