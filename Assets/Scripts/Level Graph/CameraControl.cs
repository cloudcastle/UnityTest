using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;

    float zoomSpeed = 1.1f;
    Vector3 draggingWorldPoint;

    new Camera camera;

    RaycastHit hit;
    public LevelNode hovered;

    void Awake() {
        instance = this;
    }

    void OnEnable() {
        camera = GetComponent<Camera>();
    }

    void zoom(float times) {
        var mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        var radiusVector = mouseWorldPoint - transform.position;
        transform.Translate(radiusVector * (1 - times));
        camera.orthographicSize *= times;
    }

    void Update() {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            zoom(1f / zoomSpeed);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            zoom(zoomSpeed);
        }
        if (Input.GetMouseButtonDown(1)) {
            draggingWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1)) {
            var mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            transform.Translate(draggingWorldPoint - mouseWorldPoint);
        }
        if (Input.GetMouseButtonDown(0)) {
            if (hovered != null) {
                GameManager.instance.Play(hovered.level);
            }
        }

        this.hovered = null;
        Physics.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit);
        if (hit.collider != null) {
            var hoveredNode = hit.collider.GetComponent<LevelNode>();
            if (hoveredNode != null) {
                this.hovered = hoveredNode;
            }
        }
    }
}