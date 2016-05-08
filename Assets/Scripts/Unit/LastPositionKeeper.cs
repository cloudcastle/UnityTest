using UnityEngine;
using System.Collections;

public class LastPositionKeeper : MonoBehaviour
{
    public Vector3 lastPosition;

    void FixedUpdate() {
        lastPosition = transform.position;
    }

    public void Reset() {
        lastPosition = transform.position;
    }
}