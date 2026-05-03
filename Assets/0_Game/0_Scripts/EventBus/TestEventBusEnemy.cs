using UnityEngine;

public class TestEventBusEnemy : MonoBehaviour {

    public int Points = 2;
    private void OnDisable() {
        EventBus<TestEnemyEvent>.Raise(new TestEnemyEvent { Points = this.Points});
    }

}