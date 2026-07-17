using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;

[CreateAssetMenu(menuName = "Strategy/Hero/HeroStrategy", fileName = "New Hero Strategy")]
public class HeroStrategyData : ScriptableObject {
    public Sprite Icon;
    [SerializeField] string Name;
    LocalizedString localaziedStory;
    [HideInInspector] public string Story;
 
    
    public string GetStory() {
        localaziedStory = new LocalizedString("StoryTable", Name);
        string story = localaziedStory.GetLocalizedString();
        return story;
    }


    public GameObject ModelPrefab;
    public HealtComponentData HealtComponentData;
    public ManaConponentData ManaConponentData;
    public SkillsStrategy[] SkillStrategyData;
}
