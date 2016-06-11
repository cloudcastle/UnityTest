using UnityEngine;
using System.Collections;

public class LastPositionKeeper : MonoBehaviour
{
    Vector3 prelastPosition;
    Vector3 lastPosition;
    float lastUpdateTime;

    void Start() {
        Reset();
    }

    void FixedUpdate() {
        RegisterPosition();
    }

    private void RegisterPosition() {
        prelastPosition = lastPosition;
        lastPosition = transform.position;
        lastUpdateTime = Time.fixedTime;
    }

    public void Reset() {
        prelastPosition = lastPosition = transform.position;
    }

    public Vector3 GetPreviousPosition() {
        if (lastUpdateTime > Time.fixedTime) {
            return lastPosition;
        } else {
            return prelastPosition;
        }
    }
}