using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour
{
    RaycastHit hit;

    public GameObject underSight;
    public float distance;

    void SetUnderSight(GameObject go, float distance) {
        if (go != underSight) {
            underSight = go;
            Debug.LogFormat("Under sight: {0}", go != null ? go.transform.Path() : "None");
        }
        this.distance = distance;
    }

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        bool b = Portal.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.forward)), out hit);
        if (b) {
            SetUnderSight(hit.collider.gameObject, hit.distance);
        } else {
            SetUnderSight(null, float.PositiveInfinity);
        }
    }
}