using UnityEngine;
using System.Collections;

public class SightRayCaster : MonoBehaviour
{
    RaycastHit hit;
    GameObject underSight;

    void SetUnderSight(GameObject go) {
        if (go != underSight) {
            underSight = go;
            Debug.Log("See: " + underSight);
        }
    }

    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        bool b = Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.forward)), out hit);
        if (b) {
            SetUnderSight(hit.collider.gameObject);
        } else {
            SetUnderSight(null);
        }
    }
}