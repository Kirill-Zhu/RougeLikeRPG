using UnityEngine;

public class TestEventBusHero: MonoBehaviour {

    EventBinding<TestHeroEvent> testEvent;
    EventBinding<TestEnemyEvent> OnEnemyEvent;
    private void OnEnable() {
        OnEnemyEvent = new EventBinding<TestEnemyEvent>(OnEnemyDieEvent);
        testEvent = new EventBinding<TestHeroEvent>(Foo);
        EventBus<TestHeroEvent>.Register(testEvent);
        EventBus<TestEnemyEvent>.Register(OnEnemyEvent);
    }
    private void OnDisable() {
        EventBus<TestHeroEvent>.Deregister(testEvent);
        EventBus<TestEnemyEvent>.Deregister(OnEnemyEvent);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            EventBus<TestHeroEvent>.Raise(new TestHeroEvent() { TestValue = 1, TestValue2 = 2 });
        }
    }
    void Foo(TestHeroEvent @event) {
        Debug.Log($"Event recieved  value is : {@event.TestValue}");
    }
    void OnEnemyDieEvent(TestEnemyEvent @event) {
        Debug.Log($"Enemy die event recieved  value is : {@event.Points}");
    }
}
