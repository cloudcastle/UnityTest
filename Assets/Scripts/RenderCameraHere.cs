using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class RenderCameraHere : MonoBehaviour
{
    new Camera camera;
    public RectTransform rectTransform;

    void OnEnable() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        camera.pixelRect = new Rect(
            rectTransform.position.x,
            rectTransform.position.y-rectTransform.sizeDelta.y,
            rectTransform.sizeDelta.x,
            rectTransform.sizeDelta.y
        );
    }
}