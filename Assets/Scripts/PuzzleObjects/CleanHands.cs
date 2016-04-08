using UnityEngine;
using System.Collections.Generic;


// local Y should be the axis normal to clean hands field

public class CleanHands : MonoBehaviour
{
    void OnTriggerStay(Collider other) {
        var player = other.GetComponent<Player>();
        if (player != null) {
            Vector3 localPlayerPosition = transform.InverseTransformPoint(player.transform.position);
            Vector3 previousLocalPlayerPosition = transform.InverseTransformPoint(player.lastPositionKeeper.lastPosition);
            if (Mathf.Sign(localPlayerPosition.y) != Mathf.Sign(previousLocalPlayerPosition.y)) {

                player.inventory.DropAll(player.lastPositionKeeper.lastPosition);
            }
        }
    }
}
