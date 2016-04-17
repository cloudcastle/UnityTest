using UnityEngine;
using System.Collections;

public class OnPlayerStay : AbstractTrigger
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() != null)
        {
            effect.Run();
        }
    }
}