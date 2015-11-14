using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ResetCameraMatrix : MonoBehaviour
{
    new Camera camera;

    public bool resetMatrix = false;

    void OnEnable() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        if (resetMatrix) {
            camera.ResetProjectionMatrix();
            resetMatrix = false;
        }
    }
}