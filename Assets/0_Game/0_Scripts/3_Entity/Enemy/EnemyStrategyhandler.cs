using UnityEngine;

[CreateAssetMenu(menuName ="Strategy/Enemy/EnemyStratgyHandler", fileName = "EnemyTypes")]
public class EnemyStrategyhandler: ScriptableObject
{
    public EnemyStrategy commonSkelet;
    public EnemyStrategy commonShooter;
    public EnemyStrategy rareSkelet;
    public EnemyStrategy ElCactus;
    public EnemyStrategy Lich;
}