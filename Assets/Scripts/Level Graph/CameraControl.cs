using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float zoomSpeed = 1.1f;

    new Camera camera;

    void OnEnable() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            camera.orthographicSize /= zoomSpeed;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            camera.orthographicSize *= zoomSpeed;
        }
    }
}