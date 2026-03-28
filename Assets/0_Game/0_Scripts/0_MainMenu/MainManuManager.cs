using UnityEngine;
using Zenject;

public class MainManuManager : MonoBehaviour
{
    [Inject] Hero hero;
    private void Awake() {
        hero.gameObject.SetActive(false);
    }
}
