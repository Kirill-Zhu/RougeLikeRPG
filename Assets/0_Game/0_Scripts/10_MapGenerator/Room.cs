using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(BoxCollider))]
public class Room : MonoBehaviour {
    [SerializeField] PropsGenerator propsGenerator;
    [SerializeField] Transform[] spawnTransformArray;

    BoxCollider roomTrigger;
    EnemiesManager enemiesManager;
    public int roomTierIndex;

    // Addressables
    readonly string adress = "Map/Portal";

   
    private List<AsyncOperationHandle<GameObject>> activeHandles = new List<AsyncOperationHandle<GameObject>>();

    private void Awake() {
        roomTrigger = GetComponent<BoxCollider>();
        roomTrigger.isTrigger = true;
    }

    public void Initialize(EnemiesManager enemiesManager, int roomIndex) {
        this.enemiesManager = enemiesManager;
        roomTierIndex = roomIndex;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Hero>()) {
            enemiesManager.StartSpawnSession(roomTierIndex, spawnTransformArray);
            roomTrigger.enabled = false;
        }
    }

    public void GenerateProps() {
        if (propsGenerator != null)
            propsGenerator.GenerateProps();
        else {
            propsGenerator = GetComponent<PropsGenerator>();
            propsGenerator.GenerateProps();
        }

        for (int i = 0; i < spawnTransformArray.Length; i++) {
            var handle = Addressables.InstantiateAsync(adress, spawnTransformArray[i].position, spawnTransformArray[i].rotation);
            activeHandles.Add(handle);

            handle.Completed += op => {
                Debug.Log($"{op.DebugName} is instantiated ");
            };
        }
    }

    [ContextMenu("Generate portals")]
    public void GeneratePortals() {
        for (int i = 0; i < spawnTransformArray.Length; i++) {
       
            var handle = Addressables.InstantiateAsync(adress, spawnTransformArray[i].position, spawnTransformArray[i].rotation);
            activeHandles.Add(handle);

            handle.Completed += op => {
                Debug.Log($"{op.DebugName} is instantiated ");
            };
        }
    }

    private void OnDestroy() {
        foreach (var handle in activeHandles) {
            if (handle.IsValid()) {
                Addressables.Release(handle);
            }
        }
        activeHandles.Clear();
    }
}
