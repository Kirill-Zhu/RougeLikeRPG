using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;


[CreateAssetMenu(menuName = "Strategy/HealthPopUpDamge", fileName = "PopUpHealthDamage")]
public class PopUpDamageValuesHandler : ScriptableObject {
    [SerializeField] GameObject canvasPrefab;
    [SerializeField] GameObject textMeshPrefab;
    List<TextMeshProUGUI> textMesh = new List<TextMeshProUGUI>();
    GameObject canvas;
    CancellationToken Token;


    Transform origin;
    public void Initialize(Transform origin, CancellationToken token) {
        this.origin = origin;
        this.Token = token;
        canvas = Instantiate(canvasPrefab, null);
        canvas.transform.localPosition = Vector3.zero;

        for (int i = 0; i < 5; i++) {
            var textM = Instantiate(textMeshPrefab, canvas.transform);
            textMesh.Add(textM.GetComponent<TextMeshProUGUI>());
            textM.gameObject.SetActive(false);
            // Debug.Log($"Text mesh count is {textMesh.Count}");
        }
    }
    private void OnDestroy() {
        foreach (var obj in textMesh)
            Destroy(obj);
    }
    [ContextMenu("Pop Up Damage")]
    public async void PupUpDamage(DamageType damageType, int recievedDamage) {

        foreach (var textMesh in textMesh) {
            if (!textMesh.gameObject.activeInHierarchy) {

                textMesh.transform.position = origin.position + Vector3.up;
                textMesh.transform.rotation = Quaternion.identity;
                textMesh.gameObject.SetActive(true);
                textMesh.text = recievedDamage.ToString();
                textMesh.color = damageType;

                Vector3 endPos = origin.up * damageType;
                await textMesh.transform.DOMoveY(endPos.y, 1);
                DeactiveTextMesh(textMesh);

                if (Token.IsCancellationRequested) {
                    Destroy(canvas.gameObject);
                    return;
                }
                break;
            }
        }

    }

    public async void PopUpBlock(DamageType damageType) {

        foreach (var textMesh in textMesh) {
            if (!textMesh.gameObject.activeInHierarchy) {
                textMesh.transform.position = origin.position + Vector3.up;
                textMesh.transform.rotation = Quaternion.identity;
                textMesh.gameObject.SetActive(true);
                textMesh.text = "Block!";
                textMesh.color = damageType;
                Vector3 endPos = origin.up * damageType;


                await textMesh.transform.DOMoveY(endPos.y, 1);
                DeactiveTextMesh(textMesh);

                if (Token.IsCancellationRequested) {
                    Destroy(canvas.gameObject);
                    return;
                }
                break;
            }
        }
    }

    void DeactiveTextMesh(TextMeshProUGUI textMesh) {

        //Because of old link may cause null ref error
        if (textMesh != null && canvas != null) {
            textMesh.gameObject.SetActive(false);
            canvas.transform.localPosition = Vector3.zero;
        }

    }
}
