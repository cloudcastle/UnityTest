using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LevelEdge : MonoBehaviour
{
    public Material baseMaterial;

    public LevelNode from;
    public LevelNode to;

    public SpriteRenderer sprite;
    public LineRenderer line;

    public void SetEmission(Color emission) {
//        renderers.ToList().ForEach(r => {
//            if (r.material.GetColor("_EmissionColor") != emission) {
//                r.material.SetColor("_EmissionColor", emission);
//            }
//        });
    }

    void Start() {
        from.Start();
        to.Start();
        Update();
    }

    public void SetVisible(bool visible) {
        sprite.enabled = false;
        line.enabled = visible;
    }

    void Update() {
        Vector3 dir = to.transform.position - from.transform.position;
        float len = dir.magnitude;
        float baseWidth = 0.025f;
        line.startWidth = from.transform.localScale.x * baseWidth;
        line.endWidth = to.transform.localScale.x * baseWidth;

        if (from != null && to != null) {
            transform.position = (from.transform.position + to.transform.position) / 2;
            //transform.LookAt(to.transform);
            transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg);
            transform.localScale = new Vector3(len, 1, 1);
            gameObject.name = string.Format("{0} - {1}", from.name, to.name);
        }
        if (Extensions.Editor()) {
        } else {
            if (to.Hovered()) {
                SetEmission(Color.white);
            } else if (from.Hovered()) {
                SetEmission(Color.yellow);
            } else {
                SetEmission(from.Emission());
            }
            SetVisible(from.IsVisible() && to.IsVisible());
        }
    }
}