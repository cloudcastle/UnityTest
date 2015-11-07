using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Sticky : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.9f) 
        {
            if (hit.transform != transform.parent)
            {
                transform.SetParent(hit.transform, true);
                Debug.Log("Parented to " + hit.gameObject.Path());
                Debug.Log("Global scale is now " + transform.lossyScale);
            }
        }
    }
}