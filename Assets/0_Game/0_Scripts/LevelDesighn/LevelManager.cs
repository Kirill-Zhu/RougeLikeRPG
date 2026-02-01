using UnityEngine;
using Zenject;
public class LevelManager: MonoBehaviour {
    [Inject]
    public Hero hero;
    public HeroSpawner HeroSpawner;
    private void Awake() {
       HeroSpawner = new HeroSpawner(hero);
        ActivateScene();
    }
    [ContextMenu("Activate Hero")]
    public void ActivateScene() {
        HeroSpawner.SpawnHero();
    }
}

[System.Serializable]
public class HeroSpawner {

    Hero hero;
    public HeroSpawner (Hero hero) {
        this.hero = hero;
    }
    public void SpawnHero() {
        hero.transform.position = Vector3.zero;
        hero.transform.rotation = Quaternion.identity;
        hero.gameObject.SetActive(true);
    }
}