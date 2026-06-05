using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using R3;
public class CloudSaveManager : MonoBehaviour {

    public ReactiveProperty<int> Coins = new(0);
    static ISet<string> keys = new HashSet<string>() { "Coins" };

    private void Awake() {
        Coins.Subscribe(value => {
            EventBus<OnChangeData>.Raise(new OnChangeData { Coins = value });
            GameData.SetCoinsCount(value);
            }
        );
    }
    [ContextMenu("Save Data")]
    public void SaveData() {
        var data = new Dictionary<string, object> { { "Coins", (int)Coins.Value } };
        CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    [ContextMenu("Load Data")]
    public async UniTask LoadData() {
        Debug.Log("Load Cloud Data");
        await UnityServices.InitializeAsync();
        var laodedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (laodedData.TryGetValue("Coins", out var loadedName)) {
            Coins.Value = loadedName.Value.GetAs<int>();

        } else {
            Debug.LogWarning("Cant load data");
        }
    }
}