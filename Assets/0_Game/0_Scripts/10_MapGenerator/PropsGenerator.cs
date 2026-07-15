using System.Linq;
using UnityEngine;
public class PropsGenerator : MonoBehaviour {
    [SerializeField] int minPorpsCount;
    [SerializeField] GameObject[] props;
    [SerializeField] Transform[] propsPositionArray;

    public void GenerateProps() {
        int propsCount = Random.Range(minPorpsCount, propsPositionArray.Length);
       
        int[] posIndexArray = new int[propsCount];
        for (int i = 0; i < propsCount; i++) {
            int randomIndex = Random.Range(0, propsCount);
            if (posIndexArray.Contains(randomIndex)){
                propsCount++;
                continue;
            }
            posIndexArray[i] = randomIndex;
            
            var obj = Instantiate(props[Random.Range(0, props.Length)]);
            obj.transform.position = propsPositionArray[randomIndex].position;
            obj.transform.rotation = propsPositionArray[randomIndex].rotation;
            obj.transform.SetParent(transform);
        }
    }
}
