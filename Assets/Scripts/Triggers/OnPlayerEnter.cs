using UnityEngine;
using System.Collections;

public class OnPlayerEnter : AbstractTrigger
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            effect.Run();
        }
    }
}