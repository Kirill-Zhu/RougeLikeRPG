using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent (typeof(BoxCollider))]
public class Room : MonoBehaviour
{
    [SerializeField] PropsGenerator propsGenerator;
    [SerializeField] Transform[] spawnTransformArray;

    
    BoxCollider roomTrigger;
    EnemiesManager enemiesManager;
    public int roomTierIndex;
    //Addressables
    readonly string adress = "Map/Portal";
    AsyncOperationHandle<GameObject> instanceHandle;
    private void Awake() {
        roomTrigger = GetComponent<BoxCollider>();
        roomTrigger.isTrigger = true;
    }
    public void Initialize(EnemiesManager enemiesManager, int roomIndex) {
        this.enemiesManager = enemiesManager;
        roomTierIndex = roomIndex;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Hero>()) {
            enemiesManager.StartSpawnSession(roomTierIndex, spawnTransformArray);
            roomTrigger.enabled = false;
        }
        
    }
    public void GenerateProps() {
        try {
            propsGenerator.GenerateProps();
        }
        catch {
            Debug.LogWarning("No props generator");
        }

        for(int i = 0; i < spawnTransformArray.Length; i++) {

            instanceHandle = Addressables.InstantiateAsync(adress, spawnTransformArray[i].position, spawnTransformArray[i].rotation);
            instanceHandle.Completed += op => {
                Debug.Log($"{op.DebugName} is instantialted ");
            };
        }

    }
    [ContextMenu("Generate portals")]
    public void GeneratePortals() {
        for (int i = 0; i < spawnTransformArray.Length; i++) {

            instanceHandle = Addressables.InstantiateAsync(adress, spawnTransformArray[i].position, spawnTransformArray[i].rotation);
            instanceHandle.Completed += op => {
                Debug.Log($"{op.DebugName} is instantialted ");
            };
        }
    }

    private void OnDestroy() {
        Addressables.Release(instanceHandle);
    }
}
