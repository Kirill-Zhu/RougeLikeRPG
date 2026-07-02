using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RandomCarousel : MonoBehaviour {

    [SerializeField] bool carouselIsSpin;
    [SerializeField] GameObject prefab;
    [SerializeField] Vector2 stratPos;
    [SerializeField] Vector2 endPos;
    [SerializeField] Vector2 finalItemPos;
    [SerializeField] List<SkillPowerUp> powerUpList;
    List<IItem> itemList = new List<IItem>();
    int randomSeed;
    float spawnRate = 0.4f;
    float lerpDuration = 1;
    CancellationTokenSource cts = new CancellationTokenSource();
    CancellationToken token;
    private void Awake() {
        itemList.Add(powerUpList[0]);
        itemList.Add(powerUpList[1]);
    }
    [ContextMenu("Start Carousel")]
    async void StartCarousel() {
        carouselIsSpin = true;
        cts = new CancellationTokenSource();
        token = cts.Token;
        while (carouselIsSpin) {

            for (int i = 0; i < itemList.Count; i++) {
                token.ThrowIfCancellationRequested();
                SpawnItem(i);
                await UniTask.WaitForSeconds(spawnRate);
            }
        }
    }
    [ContextMenu("Stop Carousel")]
    public async void StopCarousel() {
        carouselIsSpin = false;
        await UniTask.WaitForSeconds(spawnRate);
        randomSeed = Random.Range(0, itemList.Count);
        for (int i = 0; i <= randomSeed; i++) {
            token.ThrowIfCancellationRequested();
            if (i == randomSeed) {
                SpawnFinalItem(i);
                return;
            }
            SpawnItem(i);
            await UniTask.WaitForSeconds(spawnRate, ignoreTimeScale: true, cancellationToken: token);
        }
    }

    async void SpawnFinalItem(int index) {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform);

        var image = go.GetComponent<Image>();
        image.sprite = itemList[index].Icon;

        var rectTransofrm = go.GetComponent<RectTransform>();
        rectTransofrm.anchoredPosition = stratPos;
        await rectTransofrm.DOAnchorPos(finalItemPos, 2).SetEase(Ease.InBounce).ToUniTask();
    }
    async void SpawnItem(int index) {
        Debug.Log("Spawn Item");
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform);

        var image = go.GetComponent<Image>();
        image.sprite = itemList[index].Icon;

        var rectTransofrm = go.GetComponent<RectTransform>();
        rectTransofrm.anchoredPosition = stratPos;
        await rectTransofrm.DOAnchorPos(endPos, lerpDuration).SetEase(Ease.Linear).ToUniTask();

        Destroy(go);
    }
}
