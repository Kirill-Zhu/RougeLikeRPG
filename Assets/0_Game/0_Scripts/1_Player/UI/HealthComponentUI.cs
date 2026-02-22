using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class HealthComponentUI : MonoBehaviour {
    [SerializeField] CanvasGroup bloodBoundriesCanvasGroup;
    [SerializeField] List<Color> physicsGradientColors;
    [SerializeField] List<Color> fireGradientColors;
    [SerializeField] List<Color> coldGradientColors;
    [SerializeField] RectTransform ancorTransform;
    [SerializeField] TextMeshProUGUI TakeDamageTextMeshPrfab;
    List<TextMeshProUGUI> valuesList = new List<TextMeshProUGUI>();


    Sequence sequence;

    private void Awake() {
        Initialize();
    }
    void Initialize() {

        for (int i = 0; i < 16; i++) {
            var obj = Instantiate(TakeDamageTextMeshPrfab, ancorTransform);
            valuesList.Add(obj.GetComponent<TextMeshProUGUI>());
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.localPosition += new Vector3(UnityEngine.Random.Range(-40 , 70), UnityEngine.Random.Range(-50, 50));
            obj.gameObject.SetActive(false);
        }
    }
   
    public void PopUpDamagePoints(DamageType damageType) {

        HandleDamageAnimation(damageType);
        AnimateBloodBoundries();
        
    }
    void AnimateBloodBoundries() {
      
        if (bloodBoundriesCanvasGroup.alpha == 0) {
            bloodBoundriesCanvasGroup.alpha = 1;
            DOTween.To(() => bloodBoundriesCanvasGroup.alpha, x => bloodBoundriesCanvasGroup.alpha = x,0,0.5f).OnComplete(()=> bloodBoundriesCanvasGroup.alpha = 0);
        }
    }
    void HandleDamageAnimation(object o) {

        MethodInfo visitMethodInfo = GetType().GetMethod("HandleDamageAnimation", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { o.GetType() }, null);
        if (visitMethodInfo != null && visitMethodInfo != GetType().GetMethod("HandleDamageAnimation", new Type[] { typeof(object) })) {
            visitMethodInfo.Invoke(this, new object[] { o });
        }

    }


    void HandleDamageAnimation(PhysicsDamageType damageType) {

        foreach (var text in valuesList) {
            if (text.isActiveAndEnabled) continue;

            text.gameObject.SetActive(true);
            text.text = damageType.Value.ToString();
            //Color
            VertexGradient gradient = new VertexGradient(physicsGradientColors[0], physicsGradientColors[1], physicsGradientColors[2], physicsGradientColors[3]);
            text.colorGradient = gradient;

            //Animation
            Vector3 startPos = text.GetComponent<RectTransform>().localPosition;
            text.rectTransform.DOMoveY(400, 1).SetEase(Ease.InCirc).OnComplete(() => { text.gameObject.SetActive(false); text.rectTransform.localPosition = startPos; });
            break;
        }


        Debug.Log($"handle UI animation {this.GetType().Name} by Type {damageType}");
    }
    void HandleDamageAnimation(FireDamageType damageType) {

        foreach (var text in valuesList) {
            if (text.isActiveAndEnabled) continue;

            text.gameObject.SetActive(true);
            text.text = damageType.Value.ToString();

            //Color
            VertexGradient gradient = new VertexGradient(fireGradientColors[0], fireGradientColors[1], fireGradientColors[2], fireGradientColors[3]);
            text.colorGradient = gradient;

            //Animation
            Vector3 startPos = text.GetComponent<RectTransform>().localPosition;
            text.rectTransform.DOMoveY(400, 1).SetEase(Ease.InCirc).OnComplete(() => { text.gameObject.SetActive(false); text.rectTransform.localPosition = startPos; });
            break;
        }

        Debug.Log($"handle UI animation {this.GetType().Name} by Type {damageType}");
    }

    void HandleDamageAnimation(ColdDamageType damageType) {
        foreach (var text in valuesList) {
            if (text.isActiveAndEnabled) continue;

            text.gameObject.SetActive(true);
            text.text = damageType.Value.ToString();

            //Color
            VertexGradient gradient = new VertexGradient(coldGradientColors[0], coldGradientColors[1], coldGradientColors[2], coldGradientColors[3]);
            text.colorGradient = gradient;

            //Animation
            Vector3 startPos = text.GetComponent<RectTransform>().localPosition;
            text.rectTransform.DOMoveY(400, 1).SetEase(Ease.InCirc).OnComplete(() => { text.gameObject.SetActive(false); text.rectTransform.localPosition = startPos; });
            break;
        }

       // Debug.Log($"handle UI animation {this.GetType().Name} by Type {damageType}");
    }
}
