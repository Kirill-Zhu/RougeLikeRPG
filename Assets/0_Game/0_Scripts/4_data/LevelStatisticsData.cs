using UnityEngine;

[CreateAssetMenu(menuName = "LevelStats/LevelStatsData", fileName = "New Level stats data")]
public class LevelStatisticsData : ScriptableObject
{
    [Header("Damage recieved")]
    public int PhysicsDamageTake;
    public int FireDamageTake;
    public int ColdDamageTake;

    [Header("Damage done")]
    public int PhysicsDamageDone;
    public int FireDamageDone;
    public int ColdDamageDone;

    public void ClearData() {
        PhysicsDamageTake = 0;
        FireDamageTake = 0;
        ColdDamageTake = 0;
        PhysicsDamageDone = 0;
        FireDamageDone = 0;
        ColdDamageDone = 0;
    }
}
