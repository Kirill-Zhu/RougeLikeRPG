using Cysharp.Threading.Tasks;
using UnityEngine;

public class UpgrgadesGenerator : MonoBehaviour {
    [SerializeField] DropStrategy dropStrategy;
    [SerializeField] Transform spawnPos;

    private void OnEnable() {
        GenerateUpgradesInRoom();
    }
    public async void GenerateUpgradesInRoom() {

        //Await beacuse it rotates with room when generating map
        await UniTask.Delay(100);
        Transform parent;
        try {
            parent = transform;

        } catch {
            Debug.LogWarning("null ref parent");
            return;
        }
        GameObject tmpObj = dropStrategy.GenerateObjcet();
        tmpObj.transform.SetParent(this.transform);
        tmpObj.transform.SetPositionAndRotation(spawnPos.position, Quaternion.identity);
    }
}
