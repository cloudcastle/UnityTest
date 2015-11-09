using UnityEngine;
using System.Collections;

public class Antigravity : MonoBehaviour
{
    const float duration = 0.05f;

    public float speed = 8f;

    bool Blocked()
    {
        return Move.instance.antigravityUntil > Time.time && Move.instance.antigravitySpeed > speed;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player.instance && !Blocked())
        {
            Move.instance.antigravityUntil = Time.time + duration;
            Move.instance.antigravitySpeed = speed;
        }
    }
}