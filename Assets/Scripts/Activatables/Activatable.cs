using UnityEngine;
using UnityEngine.Events;

public abstract class Activatable : MonoBehaviour
{
    public virtual void Activate() {
        Debug.Log(string.Format("Activated: {0}", this));
    }
}