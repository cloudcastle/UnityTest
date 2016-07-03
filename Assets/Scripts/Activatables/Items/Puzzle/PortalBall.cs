using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class PortalBall : Script
{
    public Portal portal;

    new Renderer renderer;

    public override void Awake() {
        renderer = GetComponent<Renderer>();
    }

    void Update() {
        if (Extensions.Editor()) {
            Debug.LogFormat("Portal Ball Editor Update");
        }
    }
}