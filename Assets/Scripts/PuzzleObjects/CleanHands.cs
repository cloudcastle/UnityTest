using UnityEngine;
using System.Collections.Generic;


// local Y should be the axis normal to clean hands field

[ExecuteInEditMode]
public class CleanHands : MonoBehaviour
{
    // null means all items are banned
    public KeyColor bannedColor = null;
    
    public List<Colored> colored;

    float safetyDistance = 0.1f;

    bool Drop(Item item) {
        if (bannedColor == null) {
            return true;
        }
        var key = item.GetComponent<Key>();
        return key != null && key.keyColor == bannedColor;
    }

    void OnTriggerStay(Collider other) {
        var player = other.GetComponent<Unit>();
        if (player != null) {
            Vector3 localPlayerPosition = transform.InverseTransformPoint(player.transform.position);
            Vector3 previousLocalPlayerPosition = transform.InverseTransformPoint(player.lastPositionKeeper.GetPreviousPosition());
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
                player.inventory.DropAll(transform.TransformPoint(dropLocalPosition), Drop);
                player.inventory.pickStun.StartCooldown();
            }
        }
    }

    void Update() {
        if (Extensions.Editor()) {
            if (bannedColor != null) {
                Color color = bannedColor.color;
                color.a = 0.25f;
                Color emissionColor = (bannedColor.color + Color.white) / 2 / 2;
                colored.ForEach(c => {
                    c.color = color;
                    c.emissionColor = emissionColor;
                    c.setEmissionColor = true;
                    c.Update();
                });
            }
        }
    }
}
