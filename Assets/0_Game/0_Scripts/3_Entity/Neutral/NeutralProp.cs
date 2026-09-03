using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NeutralProp : MonoBehaviour {
    [SerializeField] HealtComponentData healtComponentData;
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] DropStrategy dropStrategy;
    [SerializeField] ParticleSystem takeLethalDamageVFX;
    [SerializeField] ParticleSystem dieVFX;
    [SerializeField] EventReference dieSFX;
    [SerializeField] float dieAwait = 1;

    int dieAnim = Animator.StringToHash("Die");
    Animator animator;
    private void Awake() {
        healthComponent.Initialize(healtComponentData);
        healthComponent.OnDie += Die;

        animator = GetComponent<Animator>();
    }

    protected async virtual void Die() {
        if (takeLethalDamageVFX != null) {
            takeLethalDamageVFX.gameObject.SetActive(true);
            takeLethalDamageVFX.Play();
        }
        animator.CrossFade(dieAnim, 0.1f);
        await UniTask.WaitForSeconds(dieAwait);
        if (dropStrategy != null) 
            dropStrategy.DropObject(transform.position.WithY(0));
        
        if (dieVFX != null) {
            dieVFX.gameObject.transform.SetParent(null);
            dieVFX.gameObject.SetActive(true);
            dieVFX.Play();
        }

        if (dieVFX != null)
            RuntimeManager.PlayOneShot(dieSFX, transform.position);

        Destroy(gameObject);
    }
}
