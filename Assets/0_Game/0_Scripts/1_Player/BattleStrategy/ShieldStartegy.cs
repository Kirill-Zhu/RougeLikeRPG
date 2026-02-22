using Cysharp.Threading.Tasks;
using System;
using System.Reflection;
using System.Threading;
using UnityEngine;
[CreateAssetMenu(menuName = "SkillStrategy/Shield", fileName = " Shield")]
public class ShieldStartegy : SkillsStrategy, IVistor {
    public float LiveDuration = 5;
    private bool onCoolDown = false;

    //Visitor
    CancellationTokenSource cts = new CancellationTokenSource();
    CancellationToken token;
    public override int CurrentAnimationHash { get => Animator.StringToHash(AnimationName); set => throw new NotImplementedException(); }
    public override void Initialize(Transform origin) {
        Origin = origin;
        damageTypesArray = GetDamageTypes();
    }
    public override void OnUpdate(float deltaTime) {
        if (!onCoolDown) return;

        coolDownTimer -= deltaTime;
        InvokeOnCoolDownCall(coolDownTimer / CoolDown);

        if (coolDownTimer <= 0) onCoolDown = false;
    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int,float> OnAnimation) {

        if (onCoolDown) {
            Debug.Log($"Skill {GetType().Name} on coolDown");
            OnChangeSkillDuration(0);                                           //Need to pass value or it be 1 sec in Battle Controller
            return;
        }
        //Handle coolDowns
        OnChangeSkillDuration(SkillDuration);
        onCoolDown = true;
        coolDownTimer = CoolDown;

        var spellObject = Instantiate(prefab, Origin.position, Origin.rotation);
        spellObject.transform.SetParent(Origin.transform, true);
        Destroy(spellObject, LiveDuration);

        //Visitor
        Visit(HealthComponent);

    }

    public override void UpdateValues() {
        if (damageTypesArray != null) damageTypesArray = null;

        damageTypesArray = GetDamageTypes();
    }

    //Visitor
    public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Visit : {o.GetType().Name} ");
        }
    }
    public async void Visit(HealthComponent healthComponent) {
        cts?.Cancel();
        cts = new CancellationTokenSource();
        token = cts.Token;
       
        DamageType[] damageTypesTmp = GetDamageTypes(); //Copy damage types for non conflict with taking powerUp

        for (int i = 0; i < damageTypesArray.Length; i++) {
            healthComponent.SetShieldProtection(damageTypesTmp[i]);
        }
        try {
            await UniTask.WaitForSeconds(LiveDuration, false, PlayerLoopTiming.Update, token);
            for (int i = 0; i < damageTypesArray.Length; i++) {
                healthComponent.RemoveShieldProtection(damageTypesTmp[i]);
            }
        } finally {
            cts?.Dispose();
            damageTypesTmp = null;
            cts = null;
        }
        
        Debug.Log($"VIsited {healthComponent.GetType().Name} : damge types array count is : {damageTypesArray.Length}");
    }


}
