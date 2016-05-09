using UnityEngine;
using System.Collections.Generic;
using System;

// local Y should be the axis normal to field
public class Portal : MonoBehaviour
{
    public Portal other;

    public PortalSurface front;
    public Transform back;
    new public Camera camera;
    public List<Camera> cameras;

    static List<Matrix4x4> allPortalMatrices = new List<Matrix4x4>();
    public static bool allPortalMatricesValid = false;

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

    public static bool Raycast(Ray ray, out RaycastHit hit, Action<PortalSurface> onTeleported = null) {
        bool result = Physics.Raycast(ray, out hit);
        if (hit.collider == null) {
            return result;
        }
        var portalSurface = hit.collider.GetComponent<PortalSurface>();
        if (portalSurface == null) {
            return result;
        }
        if (onTeleported != null) {
            onTeleported(portalSurface);
        }
        return Raycast(portalSurface.portal.TeleportRay(ray), out hit, onTeleported);
    }

    public Vector3 TeleportPoint(Vector3 point) {
        return other.back.TransformPoint(front.transform.InverseTransformPoint(point));
    }

    public Vector3 TeleportDirection(Vector3 direction) {
        return other.back.TransformDirection(front.transform.InverseTransformDirection(direction));
    }

    public Matrix4x4 TeleportMatrix() {
        return other.back.localToWorldMatrix * front.transform.worldToLocalMatrix;
    }

    public Ray TeleportRay(Ray ray) {
        return new Ray(TeleportPoint(ray.origin), TeleportDirection(ray.direction));
    }

    static void PrecalculateAllPortalMatrices() {
        allPortalMatrices = new List<Matrix4x4>();
        //FindObjectOfType<PortalSurface>().transform.ma
    }
}
