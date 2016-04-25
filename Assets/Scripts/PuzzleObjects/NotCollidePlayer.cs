using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NotCollidePlayer : MonoBehaviour
{
    Collider unit;
    Collider me;

    void Start() {
        unit = FindObjectOfType<Unit>().GetComponent<Collider>();
        me = GetComponent<Collider>();
    }

    void FixedUpdate() {
        Extensions.IgnoreCollision(me, unit);
    }
}
