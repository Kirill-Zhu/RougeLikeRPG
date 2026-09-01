using System;
using UnityEngine;

public class TakeDamageReactionController : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] HealtComponentData healtComponentData;
    [SerializeField] Animator animator;
    readonly int takeDamage = Animator.StringToHash("TakeDamage");
    private void OnEnable() {
        healthComponent.Initialize(healtComponentData);
        healthComponent.OnTakeDamage += AnimateDamage;
        healthComponent.OnDie += DIe;
    }

    private void AnimateDamage(DamageType arg0, int arg1) {
        animator.CrossFade(takeDamage, 0.1f);
    }
    void DIe() {
        Destroy(this.gameObject);
    }
}
