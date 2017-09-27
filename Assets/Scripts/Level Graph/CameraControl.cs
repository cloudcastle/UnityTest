using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;

    const float keyboardSpeed = 1;

    float zoomSpeed = 1.1f;
    Vector3 draggingWorldPoint;

    float currentZoom = 1;

    float minZoom = 0.1f;
    float maxZoom = 10f;

    Rect cameraBounds = new Rect(-10, -10, 20, 20);

    new Camera camera;

    RaycastHit hit;
    public LevelNode hovered;

    public List<LevelNode> nodes;

    Substitution levelNameSubstitution;
    Substitution levelStatusSubstitution;

    void Awake() {
        instance = this;
        nodes = FindObjectsOfType<LevelNode>().ToList();
        cameraBounds.xMin = nodes.ExtMin(n => n.transform.position.x);
        cameraBounds.xMax = nodes.ExtMax(n => n.transform.position.x);
        cameraBounds.yMin = nodes.ExtMin(n => n.transform.position.y);
        cameraBounds.yMax = nodes.ExtMax(n => n.transform.position.y);
    }

    void Start() {
        levelNameSubstitution = DynamicTextManager.instance.Substitute("#{hoveredLevelName}", () => hovered == null ? "" : hovered.levelName);
        levelStatusSubstitution = DynamicTextManager.instance.Substitute("#{hoveredLevelStatus}", () => {
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
        move(GameManager.game.levelGraphCameraPosition - transform.position.xy());
        bareZoom(GameManager.game.levelGraphCameraZoom / currentZoom);
    }

    void zoom(float times) {
        var mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        var radiusVector = mouseWorldPoint - transform.position;
        move(radiusVector * (1 - times));
        bareZoom(times);
    }

    void bareZoom(float times) {
        camera.orthographicSize *= times;
        currentZoom *= times;
    }

    void zoomLimited(float times) {
        float oldZoom = currentZoom;
        float newZoom = currentZoom * times;
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        zoom(newZoom / oldZoom);
    }

    void move(Vector2 delta) {
        Vector2 newPosition = transform.position.xy() + delta;
        newPosition = new Vector2(
            Mathf.Clamp(newPosition.x, cameraBounds.xMin, cameraBounds.xMax),
            Mathf.Clamp(newPosition.y, cameraBounds.yMin, cameraBounds.yMax)
        );
        transform.Translate(newPosition - transform.position.xy());
    }

    void Update() {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            zoomLimited(1f / zoomSpeed);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            zoomLimited(zoomSpeed);
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
            draggingWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
            var mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            move(draggingWorldPoint - mouseWorldPoint);
        }
        if (Input.GetMouseButtonDown(0)) {
            if (hovered != null) {
                if (hovered.level.Unlocked() || Cheats.on) {
                    GameManager.instance.Play(hovered.level);
                }
            }
        }
        move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * keyboardSpeed * currentZoom);
        var oldHovered = hovered;
        Vector3 mouse = camera.ScreenToWorldPoint(Input.mousePosition);
        LevelNode closest = nodes.Where(n => n.IsVisible()).MinBy(n => Vector2.Distance(n.basePosition, mouse));
        LevelNode closest2 = nodes.Where(n => n.IsVisible() && n != closest).MinBy(n => Vector2.Distance(n.basePosition, mouse));

        if (Vector2.Distance(closest.basePosition, mouse) < 0.9f * Vector2.Distance(closest2.basePosition, mouse)) {
            hovered = closest;
        } else {
            hovered = null;
        }

//        Physics.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit);
//        if (hit.collider != null) {
//            var hoveredNode = hit.collider.GetComponent<LevelNode>();
//            if (hoveredNode != null) {
//                this.hovered = hoveredNode;
//            }
//        }
        if (oldHovered != hovered) {
            levelNameSubstitution.Recalculate();
            levelStatusSubstitution.Recalculate();
            DynamicTextManager.instance.Invalidate();
        }
    }

    void OnDisable() {
        GameManager.game.levelGraphCameraPosition = transform.position;
        GameManager.game.levelGraphCameraZoom = currentZoom;
        GameManager.instance.Save();
    }
}