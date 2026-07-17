using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PropsGenerator : MonoBehaviour {
    [SerializeField] int minPorpsCount;
    [SerializeField] GameObject[] props;
    [SerializeField] Transform[] propsPositionArray;
    List<GameObject> generatedProps = new List<GameObject>();
    public void GenerateProps() {
        int propsCount = Random.Range(minPorpsCount, propsPositionArray.Length);
       
        int[] posIndexArray = new int[propsCount];
        for (int i = 0; i < propsCount; i++) {
            int randomPosIndex = Random.Range(0, propsPositionArray.Length);
            if (posIndexArray.Contains(randomPosIndex)){
                i--;
                continue;
            }
            posIndexArray[i] = randomPosIndex;
            
            var obj = Instantiate(props[Random.Range(0, props.Length)]);
            obj.transform.position = propsPositionArray[randomPosIndex].position;
            obj.transform.rotation = propsPositionArray[randomPosIndex].rotation;
            obj.transform.SetParent(transform);
            generatedProps.Add(obj);    
        }
    }

    private void OnDestroy() {
        if (generatedProps == null) return;

        foreach (var obj in generatedProps) { 
           Destroy(obj.gameObject);
        }
        generatedProps.Clear();
    }
}
