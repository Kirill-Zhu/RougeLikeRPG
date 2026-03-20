using Cysharp.Threading.Tasks;
using MyStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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

        //Initialize Dmaage Types
        damageTypesList = GetStartDamageTypes().ToList(); //Take initialized on inspectror enum values


        //-----------------------------

        BuildNewWeapon();

        //Animations ------
        //->Initialize Animation Hashes
        foreach (var animationName in AnimationNames)
            animationHash.Add(Animator.StringToHash(animationName));

        //Set animation time

        //-----------------------------

    }
    public override void Dispose() {
        if (weapon != null) Destroy(weapon.gameObject);


        Debug.Log($"DIspose {GetType().Name}");
        foreach (var particle in particleSystemArray)
            Destroy(particle.gameObject);

        foreach (var vfxObject in particleGameObjectsArray)
            Destroy(vfxObject.gameObject);
    }
   

    public override void UpdateValues() {
        BuildNewWeapon();
    }
    void BuildNewWeapon() {
        Dispose();

        //Instantiate Weapon
        weapon = new Mele.MeleBuilder(prefab)
            .FromOrigin(Origin)
            .WithOffset(new Vector3(0, 0, 1 * AttackRange))
            .WithDamageTypes(damageTypesList.ToArray())
            .WithInteractionTag(interactionTagName)
            .Build();


        weapon.gameObject.SetActive(false);


        particleSystemArray = new ParticleSystem[ParticlePrefabArray.Length];
        particleGameObjectsArray = new GameObject[ParticlePrefabArray.Length];

        for (int i = 0; i < ParticlePrefabArray.Length; i++) {
            //Instantiate prefabs
            var vfx = Instantiate(ParticlePrefabArray[i], Origin);
            vfx.SetActive(true);
            vfx.transform.rotation = Origin.rotation;


            particleGameObjectsArray[i] = vfx;

            //handle particle system
            particleSystemArray[i] = vfx.GetComponent<ParticleSystem>();
        }


        //Set minimum Skill Duration
        if (SkillDuration <= 0.1f)
            SkillDuration = 0.1f;
    }

    private void OnDestroy() {
        nodes.Clear();
        animationHash.Clear();
        //handle game objects

    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int, float> OnAnimation,UnityAction<int> OnManaChange) {
        OnSkillDuration = OnChangeSkillDuration;
        
        foreach (Node node in nodes) {
            if (node.Evaluate()) {
                attackQueueDamage.Enqueue(attackQueueDamage.Count);
                this.OnAnimation = OnAnimation;
                OnManaChange?.Invoke(-ManaCost);
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
        if (particleSystemArray != null) particleSystemArray[attackSeries - 1].Play();

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
