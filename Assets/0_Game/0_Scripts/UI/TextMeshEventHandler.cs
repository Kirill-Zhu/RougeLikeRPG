using TMPro;
using UnityEngine;

public class TextMeshEventHandler : MonoBehaviour
{
    [SerializeField] ManaComponent manaComponent;
    int Mana;
    TextMeshProUGUI textMesh;
    private void Awake() {
        textMesh = GetComponent<TextMeshProUGUI>();
        manaComponent.OnGetCurrentMana += x => { Mana = x; textMesh.text = Mana.ToString(); };
    }

}
