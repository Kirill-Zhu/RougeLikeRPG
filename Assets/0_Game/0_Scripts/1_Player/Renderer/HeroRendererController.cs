using UnityEngine;

public class HeroRendererController : MonoBehaviour {
     HeroModelHandler heroModelHandler;
     
    public void Initialize(HeroModelHandler modelhadler) {
        heroModelHandler = modelhadler;
        SetStandartHeroMaterial();
    }

    public void SetStandartHeroMaterial() {
        heroModelHandler.SetStandartMareial();
    }
    public void SetStrafeHeroMareial() {
        heroModelHandler.SetStrafeMaterial();
    }

}
