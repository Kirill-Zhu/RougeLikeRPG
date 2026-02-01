using UnityEngine;

[CreateAssetMenu(menuName = "SkillStrategy/Shield", fileName = " Shield")]
public class ShieldStartegy : SkillsStrategy {

    public float castDuration = 1.0f;
    public GameObject SpellPrefab;
    public float duration = 5;
    public float coolDown = 8;
    float coolDownTimer = default;
    private bool onCoolDown = false;

    public override float CoolDownTimer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void UseSkill(Transform origin, out float castDuration) {
        castDuration = this.castDuration;
        if (onCoolDown) {
            Debug.Log($"Skill {GetType().Name} on coolDown");
            return;
        }
        //Handle coolDowns
        onCoolDown = true;
        coolDownTimer = coolDown;

        var spellObject = Instantiate(SpellPrefab, origin.position, Quaternion.identity);
        spellObject.transform.SetParent(origin.transform, true);
        Destroy(spellObject, duration);
    }
    public override void OnUpdate(float deltaTime) {
        if (!onCoolDown) return;

        coolDownTimer -= deltaTime;

        if (coolDownTimer <= 0) onCoolDown = false;
    }
}
