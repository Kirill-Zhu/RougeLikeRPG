using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Hero/HeroStrategy", fileName = "New Hero Strategy")]
public class HeroStrategyData : ScriptableObject {
    public GameObject ModelPrefab;
    public HealtComponentData HealtComponentData;
    public SkillsStrategy[] SkillStrategyData;
}
