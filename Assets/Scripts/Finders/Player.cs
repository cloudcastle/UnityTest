using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player current;

    public Activator activator;
    public Eye eye;
    public Inventory inventory;
    public LastPositionKeeper lastPositionKeeper;

    void Awake()
    {
        current = this;
    }
}