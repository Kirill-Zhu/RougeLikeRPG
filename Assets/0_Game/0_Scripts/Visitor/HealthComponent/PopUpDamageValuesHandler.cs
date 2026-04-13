using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[CreateAssetMenu(menuName = "Strategy/HealthPopUpDamge", fileName = "PopUpHealthDamage")]
public class PopUpDamageValuesHandler : ScriptableObject {
    [SerializeField] GameObject canvasPrefab;
    [SerializeField] GameObject textMeshPrefab;
    List<TextMeshProUGUI> textMesh = new List<TextMeshProUGUI>();
    GameObject canvas;
    CancellationToken token;
    Transform origin;
    public void Initialize(Transform origin, CancellationToken token) {
        this.origin = origin;
        this.token = token;
        canvas = Instantiate(canvasPrefab, origin);
        canvas.transform.localPosition = Vector3.zero;

        for (int i = 0; i < 5; i++) {
            var textM = Instantiate(textMeshPrefab, canvas.transform);
            textMesh.Add(textM.GetComponent<TextMeshProUGUI>());
            textM.gameObject.SetActive(false);
            // Debug.Log($"Text mesh count is {textMesh.Count}");
        }
    }
    private void OnDestroy() {
        foreach(var obj in textMesh)
            Destroy(obj);
    }
    [ContextMenu("Pop Up Damage")]
    public async void PupUpDamage(DamageType damageType, int recievedDamage) {
        foreach (var textMesh in textMesh) {
            if (!textMesh.gameObject.activeInHierarchy) {

                canvas.transform.SetParent(null);
                textMesh.transform.position = origin.position + Vector3.up;
                textMesh.transform.rotation = Quaternion.identity;
                textMesh.gameObject.SetActive(true);
                textMesh.text = recievedDamage.ToString();
                textMesh.color = damageType;

                Vector3 endPos = origin.up * damageType;
                UniTask task = textMesh.transform.DOMoveY(endPos.y, 1).Play().ToUniTask(TweenCancelBehaviour.Kill, token);
                await task;
                
                if (token.IsCancellationRequested)
                    return;
               
                textMesh.gameObject.SetActive(false);
                canvas.transform.SetParent(origin);
                canvas.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    public async void PopUpBlock(DamageType damgeType) {
        foreach (var textMesh in textMesh) {
            if (!textMesh.gameObject.activeInHierarchy) {

                canvas.transform.SetParent(null);
                textMesh.transform.position = origin.position + Vector3.up;
                textMesh.transform.rotation = Quaternion.identity;
                textMesh.gameObject.SetActive(true);
                textMesh.text = "Block!";
                textMesh.color = damgeType;

                UniTask task = textMesh.transform.DOMoveY(origin.up.y, 1).Play().ToUniTask(TweenCancelBehaviour.Kill, token);
                await task;

                if (token.IsCancellationRequested)
                    return;

                textMesh.gameObject.SetActive(false);
                canvas.transform.SetParent(origin);
                canvas.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }
  
  
}
