using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UISeltctionHandler : MonoBehaviour {
    Button button;
    private void Awake() {
        button = GetComponent<Button>();
    }
    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null);
        // Set the new selected object
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

}
