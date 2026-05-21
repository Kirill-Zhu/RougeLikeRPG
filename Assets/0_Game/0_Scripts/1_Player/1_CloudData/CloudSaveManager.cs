using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
public static class CloudSaveManager {

    public static int coins;
 
     static ISet<string> keys = new HashSet<string>() { "Coins" };


    [ContextMenu("Save Data")]
    public static void SaveData() {
        var data = new Dictionary<string, object> { { "Coins", (int)coins } };
        CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    [ContextMenu("Load Data")]
    public static async UniTask LoadData() {
        await UnityServices.InitializeAsync();
        var laodedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (laodedData.TryGetValue("Coins", out var loadedName)) {
            coins = loadedName.Value.GetAs<int>();
        
        } else {
            coins = 0;  
        }
    }
}