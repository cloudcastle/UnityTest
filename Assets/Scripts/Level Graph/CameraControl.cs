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

    void Start() {
        DynamicTextManager.instance.Substitute("#{hoveredLevelName}", () => hovered == null ? "" : hovered.levelName);
        DynamicTextManager.instance.Substitute("#{hoveredLevelStatus}", () => {
            if (hovered == null) {
                return "";
            }
            if (hovered.level.Completed()) {
                return "completed";
            }
            if (hovered.level.Unlocked()) {
                return "unlocked";
            }
            return "locked";
        });
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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
            draggingWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
            var mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            transform.Translate(draggingWorldPoint - mouseWorldPoint);
        }
        if (Input.GetMouseButtonDown(0)) {
            if (hovered != null) {
                if (hovered.level.Unlocked() || Cheats.on) {
                    GameManager.instance.Play(hovered.level);
                }
            }
        }
        var oldHovered = hovered;
        this.hovered = null;
        Physics.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit);
        if (hit.collider != null) {
            var hoveredNode = hit.collider.GetComponent<LevelNode>();
            if (hoveredNode != null) {
                this.hovered = hoveredNode;
            }
        }
        if (oldHovered != hovered) {
            DynamicTextManager.instance.Invalidate();
        }
    }
}