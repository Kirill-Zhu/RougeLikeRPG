using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class LvlUpCard : MonoBehaviour, IVistor {
    [SerializeField] Image back;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textMesh;
    SkillsStrategy skillStrategy;
    LevelUpStrategy skillLvlUp;

    float animationDuration = 1;
    Button button;
    UnityEvent OnChooseCard;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(Choose);
       
    }
    public void Initialize(UnityEvent OnCardsChooseEvent) {
        OnChooseCard = OnCardsChooseEvent;
    }

    public async void Rise(SkillsStrategy skillStrategy, LevelUpStrategy skillLvlUp) {
        button.enabled = false;
        this.skillStrategy = skillStrategy;
        this.icon.sprite = skillStrategy.Icon;
        back.sprite = skillStrategy.BackSprite;
        textMesh.text = skillLvlUp.GetDescription();
        this.skillLvlUp = skillLvlUp;
        //Animation
        transform.rotation = Quaternion.Euler(0, 90, 0);
        await transform.DORotate(new Vector3(0, 0, 0), animationDuration, RotateMode.FastBeyond360)
               .SetEase(Ease.Linear)
               .SetUpdate(true).ToUniTask();
        //-------------------------
        button.enabled = true;
    }
    [ContextMenu("Choose")]
    public void Choose() {
        skillStrategy.Accept(this);
        OnChooseCard?.Invoke();
    }
    public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Visit : {o.GetType().Name} ");
        }
    }

    public void Visit(MeleStrategy meleStrategy) {
        var skill = skillLvlUp as MeleSkillLevleUp;
        meleStrategy = skill.UpgradeSkill(meleStrategy);
        meleStrategy.UpdateValues();
    }
    public void Visit(ShieldStartegy shieldStrategy) {
        //?????
        var skill = skillLvlUp as ShieldSkillLelvelUp;
        shieldStrategy = skill.UpgradeSkill(shieldStrategy);
        shieldStrategy.UpdateValues();
    }
    public void Visit(ShootStrategy shootStrategy) {
        var skill = skillLvlUp as ShootSkillLevelUp;
        shootStrategy = skill.UpgradeSkill(shootStrategy);
        shootStrategy.UpdateValues();
    }
    public void Visit(AreaStrategy areaStrategy) { 
    var skill = skillLvlUp as AreaSkillLevelUp;
        areaStrategy = skill.UpgradeSkill(areaStrategy);
        areaStrategy.UpdateValues();
    }
}
