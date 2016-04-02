using UnityEngine;
using System.Collections.Generic;

public class Jumper : MonoBehaviour
{
    public float minVelocityCap = 0.5f;
    public float velocity = 15f;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<Player>() != null) {
            var move = other.gameObject.GetComponent<Move>();
            if (move.velocity.y < minVelocityCap) {
                move.velocity = move.velocity.Change(y: velocity);
            }
        }
    }
}

