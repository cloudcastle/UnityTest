using UnityEngine;
using System.Collections;

public class OnPlayerStay : MonoBehaviour
{
    public Effect effect;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            effect.Run();
        }
    }
}