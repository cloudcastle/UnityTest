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

        for (int i = 0; i <= PortalSurface.maxDepth; i++) {
            var newCamera = Instantiate(camera);
            newCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            newCamera.targetTexture.name = string.Format("Texture {0} depth {1}", name, i);
            newCamera.name = string.Format("Camera {0} depth {1}", name, i);
            newCamera.enabled = false;
            cameras.Add(newCamera);
        }
    }

    public Camera GetCamera(int depth) {
        return cameras[depth];
    }

    void OnTriggerStay(Collider other) {
        var obj = other.gameObject;
        var keeper = other.GetComponentInChildren<LastPositionKeeper>();
        if (keeper != null) {
            Vector3 localObjectPosition = transform.InverseTransformPoint(keeper.transform.position);
            Vector3 previousLocalObjectPosition = transform.InverseTransformPoint(keeper.lastPosition);
            if (Mathf.Sign(localObjectPosition.z) != Mathf.Sign(previousLocalObjectPosition.z)) {
                obj.transform.SetParent(front.transform, worldPositionStays: true);
                obj.transform.SetParent(this.other.back, worldPositionStays: false);
                obj.transform.SetParent(null, worldPositionStays: true);
            }
        }
    }

    void OnWillRenderObject() {
        Debug.LogFormat("OnWillRenderObject");
    }
}
