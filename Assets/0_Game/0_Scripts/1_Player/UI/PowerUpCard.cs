using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCard : MonoBehaviour {

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textMeshItemName;
    [SerializeField] TextMeshProUGUI textMeshDescription;
    [SerializeField] Button takeButton;
    InventoryUIController inventoryUIController;
    public void Initialize(InventoryUIController inventoryUIController) {
        this.inventoryUIController = inventoryUIController;
    }
    public void RiseUpCard(Sprite label, string desctiption, string name) {
        //Animation
        transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.FastBeyond360)
               .SetEase(Ease.Linear)
               .SetUpdate(true);

        //-------------------
        image.sprite = label;
        textMeshItemName.text = name;
        textMeshDescription.text = desctiption;

        inventoryUIController.PutItemIntoEmptySlot(image.rectTransform.position, label);
    }
}