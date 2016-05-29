using UnityEngine;
using System.Collections.Generic;

public class PortalAlwaysCheck : MonoBehaviour
{
    public Portal portal;
    public GameObject target;

    void FixedUpdate() {
        portal.Check(target);
    }
}
