using UnityEngine;
using R3;
using System;
public class NewMonoBehaviourScript : MonoBehaviour
{
    IDisposable click;
    private void Start() {
        click = Observable.EveryUpdate().
            Where(_ => Input.GetMouseButtonDown(0))
            .TimeInterval()
            .Chunk(2, 1)
            .Where(clicks => clicks[1].Interval.TotalSeconds <= 2)
            .ThrottleFirst(TimeSpan.FromSeconds(2))
            .Subscribe(_ => {
                Debug.Log("Clicsk");
            });
    }
    private void OnDestroy() {
        click.Dispose();
    }
}
