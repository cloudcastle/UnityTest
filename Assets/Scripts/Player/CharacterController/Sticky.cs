using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Sticky : MonoBehaviour
{
    /// <summary>
    /// Some object in local system at the global distance of 1 from unit's origin
    /// </summary>
    public Transform sample;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.9f) 
        {
            if (hit.transform != transform.parent)
            {
                transform.SetParent(hit.transform, true);
                //RestoreScale();

                Debug.Log("Parented to " + hit.gameObject.Path());
                Debug.Log("Global scale is now " + transform.lossyScale);
                Debug.Log("Sample distance is now " + SampleDistance());
                Debug.Log("Height is now " + GetComponent<CharacterController>().height);
                Debug.Log("Radius is now " + SampleDistance());
            }
        }
    }

    float SampleDistance()
    {
        return (transform.position - sample.transform.position).magnitude;
    }

    void RestoreScale()
    {
        transform.localScale = Vector3.one;
        transform.localScale = Vector3.one / SampleDistance();
    }
}