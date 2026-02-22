using TMPro;
using UnityEngine;

public class TextMeshEventHandler : MonoBehaviour
{
    [SerializeField] ManaComponent manaComponent;
    int Mana;
    TextMeshProUGUI textMesh;
    private void Awake() {
        textMesh = GetComponent<TextMeshProUGUI>();
        manaComponent.OnManaChange += x => { Mana = x; textMesh.text = Mana.ToString(); };
    }

}
