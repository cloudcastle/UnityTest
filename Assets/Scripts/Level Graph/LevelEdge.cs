using UnityEngine;

[ExecuteInEditMode]
public class LevelEdge : MonoBehaviour
{
    public LevelNode from;
    public LevelNode to;
    new MeshRenderer renderer;

    void OnEnable() {
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update() {
        if (Extensions.Editor()) {
            transform.position = (from.transform.position + to.transform.position) / 2;
            transform.LookAt(to.transform);
            transform.localScale = new Vector3(1, 1, (from.transform.position - to.transform.position).magnitude); 
            gameObject.name = string.Format("{0} - {1}", from.name, to.name);
            renderer.enabled = from.visible && to.visible;
        }
    }
}