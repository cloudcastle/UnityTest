using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LevelEdge : MonoBehaviour
{
    public Material baseMaterial;

    public LevelNode from;
    public LevelNode to;
    MeshRenderer[] renderers;

    void OnEnable() {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetEmission(Color emission) {
        renderers.ToList().ForEach(r => {
            if (r.material.GetColor("_EmissionColor") != emission) {
                r.material.SetColor("_EmissionColor", emission);
            }
        });
    }

    void Start() {
        from.Start();
        to.Start();
        Update();
    }

    void Update() {
        if (Extensions.Editor()) {
            if (from != null && to != null) {
                transform.position = (from.transform.position + to.transform.position) / 2;
                transform.LookAt(to.transform);
                transform.localScale = new Vector3(1, 1, (from.transform.position - to.transform.position).magnitude);
                gameObject.name = string.Format("{0} - {1}", from.name, to.name);
                renderers.ToList().ForEach(r => r.enabled = from.visible && to.visible);
            }
        } else {
            if (to.Hovered()) {
                SetEmission(Color.white);
            } else if (from.Hovered()) {
                SetEmission(Color.yellow);
            } else {
                SetEmission(from.Emission());
            }
            renderers.ToList().ForEach(r => r.enabled = from.visible && to.visible || Cheats.on);
            transform.position = (from.transform.position + to.transform.position) / 2;
            transform.LookAt(to.transform);
            transform.localScale = new Vector3(1, 1, (from.transform.position - to.transform.position).magnitude);
        }
    }
}