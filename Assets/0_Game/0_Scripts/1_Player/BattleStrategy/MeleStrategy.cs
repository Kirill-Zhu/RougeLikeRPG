using Cysharp.Threading.Tasks;
using MyStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "SkillStrategy/Mele", fileName = "Mele")]
public class MeleStrategy : SkillsStrategy {

    WeaponType weapon;

    [Header("Mele Specifics")]
    float enqueTime = 0.3f;
    public int maxAttackSeries = 4;
    [Range(0, 1)]
    public float DelayToActivateDamageDeal = 0.4f;
    int attackSeries = 0;


    Queue<int> attackQueueDamage = new Queue<int>();                                                                                                                  //Store inputs in Queue with additional damage(Still dont add additional Damage)
    public int maxQueueAttackCount = 1;

    //State Machine
    HashSet<Node> nodes = new HashSet<Node>();
    Action<int, float> OnAnimation = delegate { };
    Action<float> OnSkillDuration = delegate { };

    //Animations
    public List<string> AnimationNames;
    List<int> animationHash = new List<int>();

    //UniTask
    CancellationTokenSource cts;
    CancellationToken token;


    public override int CurrentAnimationHash { get => animationHash[attackSeries]; set => throw new System.NotImplementedException(); }


    public override void Initialize(Transform origin, AudioManager audioManager, string interactionTagName) {
        initialization = true;
        Origin = origin;
        cts = new CancellationTokenSource();
        token = cts.Token;
        //Conditions
        nodes.Add(new Node(new FuncPredicate(() => coolDownTimer <= 0)));                                                                              //First attack condition
        nodes.Add(new Node(new FuncPredicate(() => coolDownTimer > 0 && coolDownTimer < enqueTime && attackQueueDamage.Count < maxQueueAttackCount))); //Series Attack condition

        //Initialize Dmaage Types
        damageTypesList = GetStartDamageTypes().ToList(); //Take initialized on inspectror enum values

        foreach (var damageType in damageTypesList) {
            SetOrAddDamageTypeWithValues(damageType);
        }
        //-----------------------------


        //Animations ------
        //->Initialize Animation Hashes
        foreach (var animationName in AnimationNames)
            animationHash.Add(Animator.StringToHash(animationName));

        //Set animation time

        //-----------------------------

        //Audio
        this.audioManager = audioManager;

        //Tag
        this.interactionTagName = interactionTagName;

        BuildNewWeapon();

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

        BuildNewVFX();



        //Set minimum Skill Duration
        if (SkillDuration <= 0.1f)
            SkillDuration = 0.1f;

        initialization = false;
    }

    private void OnDestroy() {
        nodes.Clear();
        animationHash.Clear();
        cts.Cancel();
        //handle game objects

    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int, float> OnAnimation, UnityAction<int> OnManaChange) {

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
    //For Brain
    public override bool TryUseSkill(Action<int, float> OnAnimation) {
        foreach (Node node in nodes) {
            if (node.Evaluate()) {
                attackQueueDamage.Enqueue(attackQueueDamage.Count);
                this.OnAnimation = OnAnimation;
                return true;
            }
        }
        return false;
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

        try {

            if (attackSeries >= maxAttackSeries)
                attackSeries = 0;

            if (WithCooldown)
                coolDownTimer = CoolDown;
            else
                coolDownTimer = SkillDuration;
            OnAnimation?.Invoke(CurrentAnimationHash, SkillDuration);
            OnSkillDuration?.Invoke(SkillDuration);

            PlayCastSound();
          

            //Async do damage 
            attackSeries++;

            //Add Bous Damage Physics Damage
            weapon.AddBonusDamage(new PhysicsDamageType(attackSeries));

            //VFX
            PlayOnCastVFX();
         
            await UniTask.WaitForSeconds(SkillDuration * DelayToActivateDamageDeal, false, PlayerLoopTiming.Update, token);                                                                                                          //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Need for every attack type define delay!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            weapon.gameObject.SetActive(true);

            //VFX
            PlayOnAttackVFX();

            await UniTask.WaitForSeconds(0.1f, false, PlayerLoopTiming.Update, token);

            //Remove bonus Damage

            weapon.RemoveBonusDamage(new PhysicsDamageType(attackSeries));
            weapon.gameObject.SetActive(false);
        } catch (System.OperationCanceledException) {
            Debug.Log("Task was successfully stopped.");
        }
    }



    //Use for AutoAttack on CoolDown(For boses)
    public override bool Evauate(float distanceToHero) {
        if (coolDownTimer <= 0 && distanceToHero <= AttackRange) return true;

        return false;
    }

    protected override void BuildNewVFX() {
        //On Cast
        onCastParticleSystemArray = new ParticleSystem[OnCastParticlePrefabArray.Length];
        onCastParticleGameObjectsArray = new GameObject[OnCastParticlePrefabArray.Length];

        for (int i = 0; i < OnCastParticlePrefabArray.Length; i++) {
            //Instantiate prefabs
            var vfx = Instantiate(OnCastParticlePrefabArray[i], Origin);
            vfx.SetActive(true);
            vfx.transform.rotation = Origin.rotation;


            onCastParticleGameObjectsArray[i] = vfx;

            //handle particle system
            onCastParticleSystemArray[i] = vfx.GetComponent<ParticleSystem>();
        }

        //On Attack
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
    }

    protected override void PlayOnCastVFX() {
        if (onCastParticleSystemArray.Length > 0) onCastParticleSystemArray[attackSeries - 1].Play();
    }

    protected override void PlayOnAttackVFX() {
        if (particleSystemArray != null) particleSystemArray[attackSeries - 1].Play();
    }

    protected override void PlayCastSound() {
        audioManager?.PlayOneShot(SkillSound, Origin.position);
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
