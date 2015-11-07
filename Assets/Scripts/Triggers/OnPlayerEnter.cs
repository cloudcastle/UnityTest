using UnityEngine;
using System.Collections;

public class OnPlayerEnter : MonoBehaviour
{
    public Effect effect;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.instance)
        {
            effect.Run();
        }
    }
}