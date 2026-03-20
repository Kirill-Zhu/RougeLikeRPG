using Enemies.MyStateMachine;
using System;
using UnityEngine;

public class EnemyDieState : EnemyBaseSate {
    SimpleEnemyBattleContorller battleController;
    public EnemyDieState(Animator animator, Func<float> velocityEvent, SimpleEnemyBattleContorller battleController) : base(animator, velocityEvent) {
        this.battleController = battleController;
    }

    public override void OnEnter() {
        animator.CrossFade(dieHash, duration);
        battleController.OnDie();
    }

    public override void OnExit() {

    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {

    }
}

