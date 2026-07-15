using UnityEngine;

[System.Serializable]
public class HeroSpawner {

    public Transform SpawnPoint;
    public Hero hero;

    public void SpawnHero( Transform spawnPoint= null) {
        if (spawnPoint != null) this.SpawnPoint = spawnPoint;

        if (SpawnPoint != null) {
            hero.transform.position = SpawnPoint.position;
            hero.transform.rotation = SpawnPoint.rotation;
            hero.gameObject.SetActive(true);
        }
    }
}