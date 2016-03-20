using UnityEngine;
using System.Collections;

public abstract class Effect : MonoBehaviour
{
    public abstract bool Run();

    public abstract bool Ready();
}