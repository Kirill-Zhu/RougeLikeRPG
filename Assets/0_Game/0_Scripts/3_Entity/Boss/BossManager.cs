using BossEntity;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class BossManager : MonoBehaviour
{
    [Inject, SerializeField] EventManager eventManager;
    [Inject, SerializeField] Hero hero;

    [SerializeField] float timeToBoss = 120;
    [SerializeField] GameObject TestBoss;
    bool TestBossCreated = false;   

    float startTime;

    private void Start() {
        startTime = Time.time;
    }
    private void Update() {
        if (Time.time - startTime > timeToBoss)
            CreateTestBoss();
    }
    public void CreateTestBoss() {
        if (TestBossCreated) return;

        TestBossCreated = true;
        var obj = Instantiate(TestBoss);
        obj.transform.position = new Vector3(30,0,30);
        Boss boss = obj.GetComponent<Boss>();
        boss.Initialize(hero);
        eventManager.RaiseOnBossCreate(boss);
        
    }
}
