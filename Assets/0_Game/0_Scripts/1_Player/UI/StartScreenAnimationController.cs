using UnityEngine;
using Zenject;

public class StartScreenAnimationController : MonoBehaviour
{
    [Inject] Hero hero;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject heroModel;

    private void Awake() {
        heroModel = Instantiate(hero.Model);
        heroModel.transform.position = new Vector3(0, -30,0);
    }
}
