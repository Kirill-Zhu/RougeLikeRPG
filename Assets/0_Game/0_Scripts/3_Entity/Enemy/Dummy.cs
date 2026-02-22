using Enemies;
public class Dummy : Enemy {



    protected override void Awake() {
        base.Awake();

    }
    public override void TakeDamage(int damage) {
        // Debug.Log($"{this.gameObject.name} took damage {damage}");
    }
}

public interface IDamagable {
    public void TakeDamage(int damage);
}
