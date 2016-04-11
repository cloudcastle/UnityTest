using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NotCollidePlayer : MonoBehaviour
{
    Collider player;
    Collider me;

    void Start() {
        player = Player.current.GetComponent<Collider>();
        me = GetComponent<Collider>();
    }

    void FixedUpdate() {
        Physics.IgnoreCollision(me, player);
    }
}
