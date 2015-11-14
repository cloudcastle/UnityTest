using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ObliqueCamera : MonoBehaviour
{
    public Vector2 oblique;

    new Camera camera;

    void OnEnable() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        Matrix4x4 mat = camera.projectionMatrix;
        mat[0, 2] = oblique.x;
        mat[1, 2] = oblique.y;
        camera.projectionMatrix = mat;
    }
}