using UnityEngine;
using System.Collections.Generic;
using System;

// local Y should be the axis normal to field
public class Portal : MonoBehaviour
{
    public Portal other;

    public PortalSurface front;
    public GameObject closedFront;
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
        Switch(other != null);
    }

    public Camera GetCamera(int depth) {
        return cameras[depth];
    }

    void OnTriggerStay(Collider other) {
        var obj = other.gameObject;
        Check(obj);
    }

    public void Check(GameObject obj) {
        var keeper = obj.GetComponentInChildren<LastPositionKeeper>();
        if (keeper != null) {
            Vector3 localObjectPosition = transform.InverseTransformPoint(keeper.transform.position);
            Vector3 previousLocalObjectPosition = transform.InverseTransformPoint(keeper.GetPreviousPosition());
            if (Mathf.Sign(localObjectPosition.z) != Mathf.Sign(previousLocalObjectPosition.z)) {
                var rigidbody = obj.GetComponent<Rigidbody>();
                if (rigidbody != null) {
                    rigidbody.velocity = TeleportDirection(rigidbody.velocity);
                    rigidbody.angularVelocity = TeleportDirection(rigidbody.angularVelocity);
                }
                Vector3 oldScale = obj.transform.localScale;
                obj.transform.SetParent(front.transform, worldPositionStays: true);
                obj.transform.SetParent(this.other.back, worldPositionStays: false);
                obj.transform.SetParent(null, worldPositionStays: true);
                obj.transform.localScale = oldScale;
                Debug.LogFormat("{0} teleported to {1}", obj, obj.transform.position);
                keeper.Reset();
            }
        }
    }

    void OnWillRenderObject() {
        Debug.LogFormat("OnWillRenderObject");
    }

    public static bool Raycast(Ray ray, out RaycastHit hit, Action<PortalSurface> onTeleported = null, int mask = ~0) {
        bool result = Physics.Raycast(ray, out hit, float.PositiveInfinity, mask);
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
        return Raycast(portalSurface.portal.TeleportRay(ray), out hit, onTeleported, mask);
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

    public void Switch(bool on) {
        front.gameObject.SetActive(on);
        closedFront.SetActive(!on);
    }

    public void Disconnect() {
        if (other == null) {
            return;
        }
        other.other = null;
        other.Switch(false);
        other = null;
        Switch(false);
        Debug.LogFormat("Disconnected");
    }

    public void Connect(Portal other) {
        if (this.other == other) {
            return;
        }
        Disconnect();
        other.other = this;
        this.other = other;
        Switch(true);
        other.Switch(true);
        Debug.LogFormat("Connected");
    }

    static void PrecalculateAllPortalMatrices() {
        //allPortalMatrices = new List<Matrix4x4>();

        //FindObjectsOfType<PortalSurface>().ForEach(ps => {
        //    PortalNode node = ps.portalNode;
        //    Matrix4x4 mx = node.
        //});
    }
}
