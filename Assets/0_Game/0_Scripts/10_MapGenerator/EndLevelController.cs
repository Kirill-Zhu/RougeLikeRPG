using UnityEngine;

public class EndLevelController : MonoBehaviour {
    [SerializeField] BoxCollider endLevelTrigger;
    [SerializeField] int progressKey = 0;

    public void SetProgressKey(int value) {
        progressKey = value;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Hero>() != null) {
            EventBus<OnPlayerEndLevel>.Raise(new OnPlayerEndLevel());
            GameData.SetProgressKey(progressKey);
        }
    }
}
