using Cysharp.Threading.Tasks;
using MyStateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SkillStrategy/Mele", fileName = "Mele")]
public class MeleStrategy : SkillsStrategy {

    WeaponType weapon;

    public float AttackRange = 1;
    float enqueTime = 0.3f;
    public int maxAttackSeries = 4;
    int attackSeries = 0;
    public const string interactionTagName = "Enemy";

    Queue<int> attackQueueDamage = new Queue<int>();                                                                                                                  //Store inputs in Queue with additional damage(Still dont add additional Damage)
    public int maxQueueAttackCount = 1;

    //State Machine
    HashSet<Node> nodes = new HashSet<Node>();
    Action<int, float> OnAnimation = delegate { };
    Action<float> OnSkillDuration = delegate { };

    //Animations
    public List<string> AnimationNames;
    List<int> animationHash = new List<int>();
    public override int CurrentAnimationHash { get => animationHash[attackSeries]; set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform origin) {
        Origin = origin;

        //Conditions
        nodes.Add(new Node(new FuncPredicate(() => coolDownTimer <= 0)));                                                                              //First attack condition
        nodes.Add(new Node(new FuncPredicate(() => coolDownTimer > 0 && coolDownTimer < enqueTime && attackQueueDamage.Count < maxQueueAttackCount))); //Series Attack condition


        BuildNewWeapon();

        //Animations ------
        //->Initialize Animation Hashes
        foreach (var animationName in AnimationNames)
            animationHash.Add(Animator.StringToHash(animationName));

        //Set animation time

        //-----------------------------

    }
    public override void UpdateValues() {
        BuildNewWeapon();

        if (SkillDuration <= 0.05f)
            SkillDuration = 0.05f;
    }
    void BuildNewWeapon() {
        if (weapon != null) Destroy(weapon.gameObject);

        damageTypesArray = GetDamageTypes();
        //Instantiate Weapon
        weapon = new Mele.MeleBuilder(prefab)
            .FromOrigin(Origin)
            .WithOffset(new Vector3(0, 0, 1 * AttackRange))
            .WithDamageTypes(damageTypesArray)
            .WithInteractionTag(interactionTagName)
            .Build();

        weapon.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        nodes.Clear();
        animationHash.Clear();
    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int,float> OnAnimation) {
        OnSkillDuration = OnChangeSkillDuration;

        foreach (Node node in nodes) {
            if (node.Evaluate()) {
                attackQueueDamage.Enqueue(attackQueueDamage.Count);
                this.OnAnimation = OnAnimation;
                return;
            }
        }

    }
    public override void OnUpdate(float deltaTime) {
        if (coolDownTimer > 0) {
            coolDownTimer -= deltaTime;
            InvokeOnCoolDownCall(coolDownTimer / SkillDuration);
        }                                                                                                           //Update coolDown

        if (attackQueueDamage.Count > 0 && coolDownTimer <= 0) {
            UniTask task = Attack(attackQueueDamage.Dequeue());
        }

        if (coolDownTimer <= 0 && attackQueueDamage.Count <= 0) {                                                                                                     //Reset series if no inputs
            attackSeries = 0;
        }
    }
    private async UniTask Attack(float attackBonus) {
        if (attackSeries >= maxAttackSeries)
            attackSeries = 0;

        coolDownTimer = SkillDuration;
        OnAnimation.Invoke(CurrentAnimationHash, SkillDuration);
        OnSkillDuration.Invoke(SkillDuration);
        //Async do damage 
        attackSeries++;
        await UniTask.WaitForSeconds(SkillDuration * 0.4f);                                                                                                          //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Need for every attack type define delay!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        weapon.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(0.1f);
        weapon.gameObject.SetActive(false);


    }



    class Node {
        IPredicate condition;

        public Node(IPredicate condition) {
            this.condition = condition;

        }
        public bool Evaluate() {
            return condition.Evaluate();
        }

    }

}
