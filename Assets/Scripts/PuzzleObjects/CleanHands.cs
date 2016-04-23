using UnityEngine;
using System.Collections.Generic;


// local Y should be the axis normal to clean hands field

public class CleanHands : MonoBehaviour
{
    float safetyDistance = 0.1f;

    void OnTriggerStay(Collider other) {
        var player = other.GetComponent<Unit>();
        if (player != null) {
            Vector3 localPlayerPosition = transform.InverseTransformPoint(player.transform.position);
            Vector3 previousLocalPlayerPosition = transform.InverseTransformPoint(player.lastPositionKeeper.lastPosition);
            if (Mathf.Sign(localPlayerPosition.y) != Mathf.Sign(previousLocalPlayerPosition.y) || Mathf.Abs(localPlayerPosition.y) < safetyDistance) {
                var dropLocalPosition = previousLocalPlayerPosition;
                if (previousLocalPlayerPosition.y > 0) {
                    if (dropLocalPosition.y < safetyDistance) {
                        dropLocalPosition.y = safetyDistance;
                    }
                } else {
                    if (Mathf.Abs(dropLocalPosition.y) < safetyDistance) {
                        dropLocalPosition.y = -safetyDistance;
                    }
                }
                player.inventory.DropAll(transform.TransformPoint(dropLocalPosition));
                player.inventory.pickStun.StartCooldown();
            }
        }
    }
}
