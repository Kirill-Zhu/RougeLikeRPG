using FMODUnity;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public PowerUp powerup;
    EventReference pickUpSound;
    GameObject particle;
    const string tagInteractionName = "Player";
    private void Awake() {
        Instantiate(powerup.ModelPrefab, this.gameObject.transform);

        if (powerup.particle != null) {
            particle = Instantiate(powerup.particle, null);
            particle.transform.position = this.transform.position;
            particle.gameObject.SetActive(false);
        }

        //Sound 
        pickUpSound = powerup.PickUpSound;
    }
    private void OnTriggerEnter(Collider other) {

        if (!other.CompareTag(tagInteractionName)) return;

        var visitable = other.GetComponents<IVisitable>();

        if (visitable != null) {
            foreach (var visi in visitable)
                visi.Accept(powerup);
            if (particle != null) {
                particle.gameObject.SetActive(true);
                Destroy(this.particle, 2);
            }
            //Sound 
            RuntimeManager.PlayOneShot(pickUpSound);
            Destroy(this.gameObject);
        }
    }

}