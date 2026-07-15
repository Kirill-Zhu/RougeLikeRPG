using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Strategy/DropStrategy")]
public class DropStrategy: ScriptableObject {
    [SerializeField] List<GameObject> dropObjects;
    [SerializeField] int percentageDropChance;
    public void DropObject(Vector3 pos) {

        int random = Random.Range(0, 101);

        if (percentageDropChance >= random) {
            int randomIndex = Random.Range(0,dropObjects.Count);
            Instantiate(dropObjects[randomIndex],pos, Quaternion.identity);
        }
    }
    public GameObject GenerateObjcet() {
        int random = Random.Range(0, 101);

        if (percentageDropChance >= random) {
            int randomIndex = Random.Range(0, dropObjects.Count);
            return Instantiate(dropObjects[randomIndex], Vector3.zero, Quaternion.identity);
        }
        return null;
    }
}