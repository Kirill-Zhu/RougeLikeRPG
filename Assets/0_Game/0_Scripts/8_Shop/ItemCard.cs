using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour {

    public Button BuyButton => buyButton;
    [SerializeField] Button buyButton;
    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Image CoinImage;
    [SerializeField] TextMeshProUGUI costTextMesh;

   public void Initialize(string description, Sprite label, int cost) {
        textMesh.text = description;
        Image.sprite = label;
        costTextMesh.text = cost.ToString();
    }
}
