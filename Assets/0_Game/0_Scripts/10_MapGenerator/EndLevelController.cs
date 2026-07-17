using UnityEngine;

public class EndLevelController : MonoBehaviour
{
    [SerializeField] BoxCollider endLevelTrigger;


    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Hero>() != null) {
            EventBus<OnPlayerEndLevel>.Raise(new OnPlayerEndLevel());
        }
    }
}
