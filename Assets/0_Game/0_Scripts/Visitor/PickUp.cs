using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public PowerUp powerup;
    [SerializeField] Vector3 modelOffset;
    EventReference pickUpSound;
    GameObject particle;
    const string tagInteractionName = "Player";

    //Refactor then
    GameObject modelPrefb;
    private async void OnEnable() {
       
        //Sound 
        pickUpSound = powerup.PickUpSound;

        //Level Generator disables it, so i need to enable it manually
        await UniTask.Delay(100);
        modelPrefb = Instantiate(powerup.ModelPrefab, this.gameObject.transform);
        modelPrefb.SetActive(true);
        modelPrefb.transform.localPosition += modelOffset;


        if (powerup.particle != null) {
            particle = Instantiate(powerup.particle, null);
            particle.transform.position = this.transform.position;
            particle.gameObject.SetActive(false);
        }

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