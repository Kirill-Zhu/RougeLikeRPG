using UnityEngine;

public class HeroModelHandler : MonoBehaviour {
    [SerializeField] GameObject[] models;
    Material[] standartMaterialArray;
    [SerializeField] Material strafeMaterialArray;

    public void SetStandartMareial() {

        if (standartMaterialArray == null) {
            standartMaterialArray = new Material[models.Length];
            for (int i = 0; i < models.Length; i++) {
                standartMaterialArray[i] = models[i].GetComponent<Renderer>().material;
            }
        }
        for (int i = 0; i < models.Length; i++) {
            models[i].GetComponent<Renderer>().material = standartMaterialArray[i];
        }

    }
    public void SetStrafeMaterial() {
        if (strafeMaterialArray == null) {
            Debug.LogWarning($"No strafe material for {this.gameObject.name}");
        }
        for (int i = 0; i < models.Length; i++) {
            models[i].GetComponent<Renderer>().material = strafeMaterialArray;
            Debug.Log($"Set strafe material for {this.gameObject.name}");
        }
    }
}
