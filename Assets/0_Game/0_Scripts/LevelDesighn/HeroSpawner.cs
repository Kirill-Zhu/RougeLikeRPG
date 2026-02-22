using UnityEngine;

[System.Serializable]
public class HeroSpawner {

    public Transform spawnPoint;
    public Hero hero;
   
    public void SpawnHero() {
        hero.transform.position = spawnPoint.position;
        hero.transform.rotation = spawnPoint.rotation;
        hero.gameObject.SetActive(true);
    }
}