using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "SkillStrategy/Area", fileName = "Area")]
public class AreaStrategy : SkillsStrategy {
    WeaponType weapon;
    public float Radius = 2;
    public float LiveDuration = 1;
    public bool FollowHero = true;
    //VFX
    public Vector3 VFXPosOffset;

    
    public override int CurrentAnimationHash { get => Animator.StringToHash(AnimationName); set => throw new NotImplementedException(); }


    public override void Dispose() {
        if (weapon != null) Destroy(weapon.gameObject);

        foreach (var particle in OnCastParticlePrefabArray)
            Destroy(particle.gameObject);
        foreach (var vfxObject in onCastParticleGameObjectsArray)
            Destroy(vfxObject.gameObject);
        Debug.Log($"DIspose {GetType().Name}");
        foreach (var particle in particleSystemArray)
            Destroy(particle.gameObject);

        foreach (var vfxObject in particleGameObjectsArray)
            Destroy(vfxObject.gameObject);
    }

    public override bool Evauate(float distanceToHero) {
        throw new NotImplementedException();
    }

    public override void Initialize(Transform origin, AudioManager audioManager, string interactionTagName) {
        initialization = true;
        this.Origin = origin;
        //Initialize Dmaage Types
        damageTypesList = GetStartDamageTypes().ToList(); //Take initialized on inspectror enum values

        foreach (var damageType in damageTypesList) {
            SetOrAddDamageTypeWithValues(damageType);
        }
        //-----------------------------

        //Set animation time

        //-----------------------------

        //Audio
        this.audioManager = audioManager;

        //Tag
        this.interactionTagName = interactionTagName;

        BuildNewWeapon();
    }

    public override void OnUpdate(float deltaTime) {

        if (initialization) return;



        if (coolDownTimer > 0) {
            coolDownTimer -= deltaTime;
            InvokeOnCoolDownCall(coolDownTimer / CoolDown);
        }

        ////Set Pos for VFX
        if (FollowHero)
            for (int i = 0; i < particleGameObjectsArray.Length; i++) {
                particleGameObjectsArray[i].transform.position = Origin.transform.position + VFXPosOffset;
            }
    }

    public override bool TryUseSkill(Action<int, float> OnAnimation) {
        return false;
    }

    public override void TryUseSkill(Action<float> OnChangeSkillDuration, Action<int, float> OnAnimation, UnityAction<int> OnManaChangEvent) {

        OnChangeSkillDuration?.Invoke(SkillDuration);

        if (coolDownTimer > 0) return;
        OnAnimation?.Invoke(CurrentAnimationHash, SkillDuration);
        OnManaChangEvent?.Invoke(-ManaCost);
        Attack();

    }
    void Attack() {
        weapon.gameObject.SetActive(true);
        coolDownTimer = CoolDown;
        PlayOnAttackVFX();
        PlayCastSound();
    }

    void BuildNewWeapon() {

        Dispose();

        weapon = new AreaWeaponType.Builder(prefab)
           .FromOrigin(Origin)
           .WithInteractionTag(interactionTagName)
           .WithDamageTypes(damageTypesList.ToArray())
           .FollowHero(FollowHero)
           .WithLiveDuration(LiveDuration)
           .Build();

        var weaponLocalScale = weapon.transform.localScale;
        weaponLocalScale = Vector3.one * Radius * 2;
        weapon.transform.localScale = weaponLocalScale;

        weapon.gameObject.SetActive(false);

        BuildNewVFX();

        //Set minimum Skill Duration
        if (SkillDuration <= 0.1f)
            SkillDuration = 0.1f;

        initialization = false;
    }
    public override void UpdateValues() {
        BuildNewWeapon();
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

            var shape = onCastParticleSystemArray[i].shape;
            shape.radius = Radius;
            var main = onCastParticleSystemArray[i].main;
            main.duration = LiveDuration;

            DontDestroyOnLoad(onCastParticleGameObjectsArray[i]);
            onCastParticleGameObjectsArray[i].transform.SetParent(null);
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

            var shape = particleSystemArray[i].shape;
            shape.radius = Radius;
            var main = particleSystemArray[i].main;
            main.duration = LiveDuration;

            DontDestroyOnLoad(particleGameObjectsArray[i]);
            particleGameObjectsArray[i].transform.SetParent(null);
        }
    }
    //VFX
    protected override void PlayOnCastVFX() {
        foreach (var vfx in onCastParticleSystemArray) {
            vfx.gameObject.transform.position = Origin.position + VFXPosOffset;
            vfx.Play();
        }
    }

    protected override void PlayOnAttackVFX() {
        foreach (var vfx in particleSystemArray) {
            vfx.Stop();
            vfx.gameObject.transform.position = Origin.position + VFXPosOffset;
            vfx.Play();
        }
    }

    //Sound

    protected override void PlayCastSound() {
        audioManager.PlayOneShot(SkillSound, Origin.position);
    }
}
