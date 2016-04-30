using UnityEngine;
using System.Collections.Generic;

// local Y should be the axis normal to field
public class Portal : MonoBehaviour
{
    public Portal other;

    public PortalSurface front;
    public Transform back;
    new public Camera camera;
    public List<Camera> cameras;

    void Awake() {
        var renderer = GetComponentInChildren<Renderer>();
        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        renderer.material.SetTexture("_MainTex", camera.targetTexture);

        for (int i = 0; i <= PortalSurface.maxDepth; i++) {
            var newCamera = Instantiate(camera);
            newCamera.enabled = false;
            cameras.Add(newCamera);
        }
    }

    public Camera GetCamera(int depth) {
        return cameras[depth];
    }

    void OnTriggerStay(Collider other) {
        //Vector3 localPosition = transform.InverseTransformPoint(other.transform.position);
        //Vector3 previousLocalPlayerPosition = transform.InverseTransformPoint(player.lastPositionKeeper.lastPosition);
        //if (Mathf.Sign(localPlayerPosition.y) != Mathf.Sign(previousLocalPlayerPosition.y) || Mathf.Abs(localPlayerPosition.y) < safetyDistance) {
        //    var dropLocalPosition = previousLocalPlayerPosition;
        //    if (previousLocalPlayerPosition.y > 0) {
        //        if (dropLocalPosition.y < safetyDistance) {
        //            dropLocalPosition.y = safetyDistance;
        //        }
        //    } else {
        //        if (Mathf.Abs(dropLocalPosition.y) < safetyDistance) {
        //            dropLocalPosition.y = -safetyDistance;
        //        }
        //    }
        //    player.inventory.DropAll(transform.TransformPoint(dropLocalPosition), Drop);
        //    player.inventory.pickStun.StartCooldown();
    }

    void OnWillRenderObject() {
        Debug.LogFormat("OnWillRenderObject");
    }
}
