using UnityEngine;
using UnityEngine.Events;
using BossEntity;
public class EventManager : MonoBehaviour
{
    
    public UnityEvent<Boss> OnBossCreate;


    public void RaiseOnBossCreate(Boss boss) {
        Debug.Log($"Raise OnBossCreate {boss.GetType().Name}");
        OnBossCreate?.Invoke(boss);
    }
}
