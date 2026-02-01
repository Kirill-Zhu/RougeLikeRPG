using UnityEngine;

public class PickUp: MonoBehaviour {
   public PowerUp powerup;

    private void OnTriggerEnter(Collider other) {

        var visitable = other.GetComponent<IVisitable>();

        if (visitable != null) { 
            visitable.Accept(powerup);
            Destroy(this.gameObject);
        }
    }

}