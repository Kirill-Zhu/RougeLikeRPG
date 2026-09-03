using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;
public class BakedEnemiesManager : MonoBehaviour {
    Hero hero;

    //Test
    [SerializeField] GameObject mushroomPrefab;
    [SerializeField] DamageBuffer damageBuffer;
    int width = 15;
    int height = 25;
    UnityEngine.Transform[,] spawnPosArray;
    //Job
    List<Transform> transformList = new List<Transform>();
    TransformAccessArray transformAccessArray;
    NativeArray<float> speedArray;
    JobHandle jobHandle;
    [Inject]
    public void Cunstruct(Hero hero) {
        this.hero = hero;
    }

    void Awake() {
        PoolMushrooms();
    }
    private void OnDisable() {
        Dispose();
    }
    private void Update() {


        MoveJob moveJob = new MoveJob() {
            DeltaTime = Time.deltaTime,
            MovePoint = hero.transform.position,
            SpeedArray = speedArray,
        };

        jobHandle = moveJob.Schedule(transformAccessArray);
    }
    private void LateUpdate() {
        jobHandle.Complete();

    }

    public void PoolMushrooms() {
        spawnPosArray = new Transform[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++) {
                var obj = Instantiate(mushroomPrefab, new Vector3(x, 0, y), Quaternion.identity);
                Material mat = obj.GetComponent<Renderer>().material;
                mat.SetFloat("_AnimationTimeOffset", Random.Range(0, 1f));
                var health = obj.GetComponent<HealthComponent>();
                health.InitializeDamageBuffer(damageBuffer);
                transformList.Add(obj.transform);
            }
        //transformList[i].gameObject.SetActive(false);

        float[] speedList = new float[transformList.Count];

        Debug.Log($"count is {transformList.Count}");
        for (int i = 0; i < speedList.Length; i++) {
            speedList[i] = 1.0f;
        }
        speedArray = new NativeArray<float>(speedList, Allocator.Persistent);
        transformAccessArray = new TransformAccessArray(transformList.ToArray());
    }

    void Dispose() {
        if (transformAccessArray.isCreated) transformAccessArray.Dispose();
        if (speedArray.IsCreated) speedArray.Dispose();
    }
    [BurstCompile]
    struct MoveJob : IJobParallelForTransform {
        //Decided to not check activity due to perfomance
        public float DeltaTime;
        public Vector3 MovePoint;
        public NativeArray<float> SpeedArray;


        public void Execute(int index, TransformAccess transform) {

            Vector3 currentPos = transform.position.WithY(0);
            Quaternion targetRotation = Quaternion.LookRotation(MovePoint.WithY(0) - currentPos, Vector3.up);
            Vector3 forwardDirection = targetRotation * Vector3.forward;
            Vector3 nextPos = currentPos + (forwardDirection * DeltaTime * SpeedArray[index]);

            transform.SetPositionAndRotation(nextPos, targetRotation);
        }
    }
}
