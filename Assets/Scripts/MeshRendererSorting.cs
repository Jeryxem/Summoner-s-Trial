using UnityEngine;

[ExecuteInEditMode]
public class MeshRendererSorting : MonoBehaviour
{
    private void Awake() {
        this.GetComponent<MeshRenderer>().sortingOrder = 15;
    }
}
