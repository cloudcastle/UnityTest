using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerHitWatcher : MonoBehaviour
{
    void Update() {
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!enabled) {
            return;
        }
        if (hit.gameObject.name != "Ground") {
            //Debug.Log("Hit: " + hit.gameObject);
        }
    }
}