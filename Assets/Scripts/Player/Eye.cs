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
        }
        this.distance = distance;
    }

    void Update()
    {
        if (TimeManager.paused)
        {
            return;
        }
        bool b = Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.forward)), out hit);
        if (b) {
            SetUnderSight(hit.collider.gameObject, hit.distance);
        } else {
            SetUnderSight(null, float.PositiveInfinity);
        }
    }
}