using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour {
    void Start() {

        void Start() {
            // 1. Get components from children, ignoring the parent's own components
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] instances = new CombineInstance[meshFilters.Length - 1];

            int index = 0;
            for (int i = 0; i < meshFilters.Length; i++) {
                // Skip the parent object itself
                if (meshFilters[i].gameObject == gameObject) continue;

                instances[index].mesh = meshFilters[i].sharedMesh;

                // FIX: Calculate positions relative to THIS parent instead of the world origin
                instances[index].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;

                meshFilters[i].gameObject.SetActive(false); // Hide original child
                index++;
            }

            // 2. Generate and assign the combined mesh
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(instances, true, true); // true combines into a single submesh

            GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        }
    }
}