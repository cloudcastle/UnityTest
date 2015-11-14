using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraProjectionMatrix : MonoBehaviour
{
    public Matrix4x4 matrix;

    new Camera camera;

    public bool resetMatrix = false;
    public bool saveMatrix = false;

    void OnEnable() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        if (resetMatrix) {
            camera.ResetProjectionMatrix();
            matrix = camera.projectionMatrix;
            resetMatrix = false;
        }
        if (saveMatrix) {
            matrix = camera.projectionMatrix;
            saveMatrix = false;
        }
        camera.projectionMatrix = matrix;
    }
}