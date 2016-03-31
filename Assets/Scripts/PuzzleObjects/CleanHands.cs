using UnityEngine;
using System.Collections.Generic;

public class CleanHands : MonoBehaviour
{
    void OnTriggerStay(Collider other) {
        if (other.GetComponent<Player>() != null) {
            other.GetComponent<Player>().inventory.DropAll();
            other.GetComponent<Player>().activator.stun.StartCooldown();
        }
    }
}
