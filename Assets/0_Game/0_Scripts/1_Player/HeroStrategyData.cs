using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Hero/HeroStrategy", fileName = "New Hero Strategy")]
public class HeroStrategyData : ScriptableObject {
    public Sprite Icon;
    public string Story;
    public GameObject ModelPrefab;
    public HealtComponentData HealtComponentData;
    public ManaConponentData ManaConponentData;
    public SkillsStrategy[] SkillStrategyData;
}
