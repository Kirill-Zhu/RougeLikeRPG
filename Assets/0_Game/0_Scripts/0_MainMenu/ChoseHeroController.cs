using System.Collections.Generic;
using UnityEngine;

public class ChoseHeroController : MonoBehaviour {
    [SerializeField] ChoseHeroStrategy choseHeroStrategy;
    [SerializeField] List<HeroStrategyData> heroesList;
    public HeroStrategyData CurrentHeroStrategyData;
    int currentIndex = 0;

    GameObject currentModel;
    [SerializeField] Vector3 heroModelPos;
    [SerializeField] Vector3 heroRotation;

    private void Awake() {
        InstantiateModel();
        SetData();
        CurrentHeroStrategyData = heroesList[currentIndex];
    }

    void InstantiateModel() {
        if (currentModel != null) Destroy(currentModel.gameObject);
        currentModel = Instantiate(heroesList[currentIndex].ModelPrefab);
        currentModel.transform.position = heroModelPos;
        currentModel.transform.rotation = Quaternion.Euler(heroRotation);
    }
    void SetData() {
        choseHeroStrategy.HeroStrategyData = heroesList[currentIndex];
    }
    public void NextModel() {
        currentIndex++;
        try {
            InstantiateModel();
            SetData();
        } catch {
            currentIndex = 0;
            InstantiateModel();
            SetData();
        }

        CurrentHeroStrategyData = heroesList[currentIndex];
    }
    public void PreviousModel() {

        currentIndex--;
        try {
            InstantiateModel();
            SetData();
        } catch {
            currentIndex = heroesList.Count - 1;
            InstantiateModel();
            SetData();
        }

        CurrentHeroStrategyData = heroesList[currentIndex];
    }


}
