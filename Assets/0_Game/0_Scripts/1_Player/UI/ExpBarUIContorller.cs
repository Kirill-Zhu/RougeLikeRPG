using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System.Threading.Tasks;
public class ExpBarUIContorller : MonoBehaviour {

    [SerializeField] List<TextMeshProUGUI> popUpExpTextMeshList;
    [SerializeField] Slider slider;
    Vector2 startAnimationPos;
    int textStartExp = 4;
    public void Initialize(Hero hero) {
        //Events
        hero.OnGetExp.AddListener(SetCurrentExp);
        hero.OnLevelUp.AddListener(ChangeMaxExp);

        //Slider
        slider.maxValue = textStartExp;
        slider.minValue = 0;
        slider.value = 0;

    }

    private void Awake() {
        startAnimationPos = popUpExpTextMeshList[0].rectTransform.position;
    }
    void SetCurrentExp(int value, int currentExp) {
        slider.value = currentExp;
        PopUpExpValue(value);
    }
    void ChangeMaxExp(int value) { 
      slider.maxValue = value;
    }

    async void PopUpExpValue(int value) {
        Debug.Log("pop up exp");
        UniTask task;
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        foreach (var textMesh in popUpExpTextMeshList) {
            if (textMesh.gameObject.activeInHierarchy) continue;

            textMesh.gameObject.SetActive(true);
            textMesh.text = value.ToString();
            textMesh.rectTransform.position = startAnimationPos;    
            task = textMesh.rectTransform.DOLocalMoveX(textMesh.rectTransform.localPosition.x +40, 1).ToUniTask();
            
            await task;   
            textMesh.gameObject.SetActive(false);
            break;
        }
     

    }
}

