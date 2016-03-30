using UnityEngine;
using System.Collections;

public abstract class Effect : MonoBehaviour
{
    public abstract bool Run();

    public virtual bool Ready() {
        return transform;
    }
}