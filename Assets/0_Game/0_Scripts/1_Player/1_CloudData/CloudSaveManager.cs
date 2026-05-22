using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
public class CloudSaveManager: MonoBehaviour {

    public int Coins=>coins;
    int coins;
     static ISet<string> keys = new HashSet<string>() { "Coins" };


    [ContextMenu("Save Data")]
    public void SaveData() {
        var data = new Dictionary<string, object> { { "Coins", (int)coins } };
        CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    [ContextMenu("Load Data")]
    public async UniTask LoadData() {
        Debug.Log("Load Cloud Data");
        await UnityServices.InitializeAsync();
        var laodedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (laodedData.TryGetValue("Coins", out var loadedName)) {
            coins = loadedName.Value.GetAs<int>();
        
        } else {
            coins = 0;  
        }

        SetLocalData();
    }

    void SetLocalData() {
        GameData.SetCoinsCount(coins);
    }
}