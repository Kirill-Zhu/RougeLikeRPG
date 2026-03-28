using UnityEngine;
using UnityEngine.Events;
public class PowerUpMenu : MonoBehaviour {
    [SerializeField] PowerUpCard powerUpCard;
    Hero hero;
    UnityEvent OnChooseCard;
    public void Initialize(Hero hero) {
        this.hero = hero;   
        hero.OnPickUppowerUp.AddListener(RiseUpPowerUpCard);
        OnChooseCard = hero.OnChooseLelvelUpCard;
    }

    void RiseUpPowerUpCard(Sprite label, string description, string name) {
        powerUpCard.gameObject.SetActive(true);
        powerUpCard.RiseUpCard(label, description, name);
    }
    public void OnCloseCardMenu() {
        OnChooseCard?.Invoke();
        powerUpCard.gameObject.SetActive(false);
    }
}