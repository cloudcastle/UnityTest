using UnityEngine;
using System.Collections;

public class SightRayCaster : MonoBehaviour
{
    public static SightRayCaster instance;

    RaycastHit hit;

    public GameObject underSight;

    void Awake() {
        instance = this;
    }

    void SetUnderSight(GameObject go) {
        if (go != underSight) {
            underSight = go;
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