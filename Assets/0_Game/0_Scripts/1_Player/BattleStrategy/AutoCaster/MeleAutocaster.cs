using Cysharp.Threading.Tasks;
using FMODUnity;
using System.Linq;
using UnityEngine;
public enum ShootShape {
    Forward = 0,
    Spread = 1,
    Around = 2
}
[CreateAssetMenu(menuName = "SkillStrategy/AutoCaster/Mele", fileName = "new Mele Autocaster")]
public class MeleAutocaster : AutoSkillStrategy {

    WeaponType weapon;
    public override void Initialize(Transform origin) {
        this.Origin = origin;

        damageTypesList = GetStartDamageTypes().ToList(); //Take initialized on inspectror enum values

        BuildNewWeapon();
    }

    public override void OnUpdate(float deltaTime) {

        if (coolDownTimer > 0) {
            coolDownTimer -= deltaTime;
            InvokeOnCoolDownCall(coolDownTimer / coolDown);
            return;
        }

        coolDownTimer = coolDown;
        UseSKill();
        
    }

    private void BuildNewWeapon() {

        //Weapon
        weapon = new Mele.MeleBuilder(prefab)
            .FromOrigin(Origin)
            .WithDamageTypes(damageTypesList.ToArray())
            .WithPushPower(PushPower)
            .WithInteractionTag(interactionTagName)
            .Build();

        weapon.gameObject.SetActive(false);


        if (parentToHero == false)
            weapon.transform.SetParent(null);

        //VFX
        particleSystemArray = new ParticleSystem[ParticlePrefabArray.Length];
        particleGameObjectsArray = new GameObject[ParticlePrefabArray.Length];

        for (int i = 0; i < ParticlePrefabArray.Length; i++) {
            //Instantiate prefabs
            var vfx = Instantiate(ParticlePrefabArray[i], Origin);
            vfx.SetActive(true);
            vfx.transform.rotation = Origin.rotation;


            particleGameObjectsArray[i] = vfx;
            if (parentToHero == false)
                particleGameObjectsArray[i].transform.SetParent(null);

            //handle particle system
            particleSystemArray[i] = vfx.GetComponent<ParticleSystem>();


        }

        //Set minimum Skill Duration
        if (coolDown <= 0.1f)
            coolDown = 0.1f;
    }
    protected override async void UseSKill() {

        if (parentToHero == false) {
            weapon.transform.position = Origin.position;
            foreach (var obj in particleGameObjectsArray) {
                obj.transform.position = Origin.position;
            }
        }

        //VFX
        foreach (var particle in particleSystemArray) {
            particle.Play();
        }

        //Damage
        weapon.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(0.1f);
        weapon.gameObject.SetActive(false);

        //Sound
        PlayOnCastSound();
    }

    protected override void PlayOnCastSound() {
        RuntimeManager.PlayOneShot(OnCastSound, Origin.position);
    }
}