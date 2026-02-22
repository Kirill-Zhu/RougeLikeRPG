using UnityEngine;

[System.Serializable]
public abstract class WeaponType : MonoBehaviour {
    protected DamageType[] damageTypes;
    protected int damage; //Test
   [ SerializeField] protected string interactionTagName;
    protected abstract void OnTriggerEnter(Collider other);
}
public enum WeaponTypeEnum {
    mele,
    projectile
}