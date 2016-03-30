using UnityEngine;
using System.Collections;

public class OnPlayerStay : AbstractTrigger
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            effect.Run();
        }
    }
}