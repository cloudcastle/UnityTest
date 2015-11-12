using UnityEngine;
using System.Collections;

public class Antigravity : MonoBehaviour
{
    public float speed = 8f;

    void OnTriggerStay(Collider other)
    {
        AntigravityEffect ae = other.GetComponent<AntigravityEffect>();
        if (ae != null) {
            ae.Affect(speed);
        }
    }
}