using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    private async void Awake() {
        
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }
}
