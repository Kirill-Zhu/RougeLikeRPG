using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using DG.Tweening;
using UnityEngine.Events;
[RequireComponent(typeof(Button))]
public class LvlUpCard : MonoBehaviour, IVistor {
    [SerializeField] Image back;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textMesh;
    SkillsStrategy skillStrategy;
    LevelUpStrategy skillLvlUp;
    RectTransform rectTransform;

    Button button;
    UnityEvent OnChooseCard;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Choose);
    }
    public void Initialize(UnityEvent OnCardsChooseEvent) {
        OnChooseCard = OnCardsChooseEvent;
    }

    private void OnEnable() {

    }
    public void Rise(SkillsStrategy skillStrategy, LevelUpStrategy skillLvlUp) {

        transform.rotation = Quaternion.Euler(0, 90,0); 
        transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.FastBeyond360)
               .SetEase(Ease.Linear)
               .SetUpdate(true);

        this.skillStrategy = skillStrategy;
        this.icon.sprite = skillStrategy.Icon;
        back.sprite = skillStrategy.BackSprite;

        this.skillLvlUp = skillLvlUp; 
        textMesh.text = skillLvlUp.GetDescription();

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

       
        shieldStrategy.UpdateValues();
    }
    public void Visit(ShootStrategy shootStrategy) {
      

        shootStrategy.UpdateValues();

    }
}