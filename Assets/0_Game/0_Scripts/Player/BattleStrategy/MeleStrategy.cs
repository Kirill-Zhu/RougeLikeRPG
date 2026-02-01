using MyStateMachine;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillStrategy/Mele", fileName = "Mele")]
public class MeleStrategy : SkillsStrategy {
    public float skillDuration = 1.0f;
    public float Damage = 2;
    public float AttackRange = 1;
    float coolDownTimer = default;
    float enqueTime = 0.5f;
    public int maxAttackSeries = 4;
    int attackSeries = 0;

    Queue<int> attackQueueDamage = new Queue<int>();//Store Damage in Queue
    int maxQueueAttackCount = 3;
    List<Node> nodes = new List<Node>();

    public override float CoolDownTimer { get => coolDownTimer; set => throw new System.NotImplementedException(); }

    private void OnEnable() {
        nodes.Add(new Node(new FuncPredicate(() => coolDownTimer <= 0)));                                                                              //First attack condition
        nodes.Add(new Node(new FuncPredicate(() => coolDownTimer > 0 && coolDownTimer < enqueTime && attackQueueDamage.Count < maxQueueAttackCount))); //Series Attack condition

    }
    public override void UseSkill(Transform origin, out float skillDuration) {
        skillDuration = this.skillDuration;

        foreach (Node node in nodes) {
            if (node.Evaluate()) {
                node.RefreshAttackIndexQueue();
                attackQueueDamage.Enqueue(attackQueueDamage.Count);
            }
        }
    }
    public override void OnUpdate(float deltaTime) {
        if (coolDownTimer > 0) coolDownTimer -= deltaTime;                                                                                                           //Update coolDown

        if (attackQueueDamage.Count > 0 && coolDownTimer <= 0) {
            Attack(attackQueueDamage.Dequeue());
        }

        if (coolDownTimer <= 0 && attackQueueDamage.Count <= 0) {                                                                                                     //Reset series if no inputs
            attackSeries = 0;
        }
    }
    private void Attack(float attackBonus) {
        if (attackSeries >= maxAttackSeries)
            attackSeries = 0;

        coolDownTimer = skillDuration;
        Debug.Log($"Attack with damate {Damage + attackSeries}");

        attackSeries++;
    }

    class Node {

        IPredicate condition;

        public Node(IPredicate condition) {
            this.condition = condition;

        }
        public bool Evaluate() {
            return condition.Evaluate();
        }
        public void RefreshAttackIndexQueue() {

        }

    }
}
