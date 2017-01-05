using UnityEngine;
using System.Collections;

public class Eye : Ability
{
    RaycastHit hit;

    public GameObject underSight;
    public float distance;

    void SetUnderSight(GameObject go, float distance) {
        if (go != underSight) {
            underSight = go;
            //Debug.LogFormat("Under sight: {0}", go != null ? go.transform.Path() : "None");
            if (go != null) {
                var handler = go.GetComponent<IOnEyeHandler>();
                if (handler != null) {
                    handler.OnEye(unit);
                }
            }
        }
        this.distance = distance;
    }

    public GameObject GetUnderSight() {
        UpdateUnderSight();
        return underSight;
    }

    public void UpdateUnderSight() {
        bool b = Portal.Raycast(new Ray(transform.position, transform.forward), out hit, mask: ~LayerMask.GetMask("UnitProof"));
        if (b) {
            SetUnderSight(hit.collider.gameObject, hit.distance);
        } else {
            SetUnderSight(null, float.PositiveInfinity);
        }
    }

    public override void Start() {
        base.Start();
        new ValueTracker<GameObject>(go => underSight = go, () => underSight);
    }

    void Update()
    {
        if (TimeManager.Paused) 
        {
            return;
        }
        UpdateUnderSight();
    }
}