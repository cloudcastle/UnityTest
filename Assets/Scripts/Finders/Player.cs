using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static Player current;

    public static List<Player> all = new List<Player>();

    public Activator activator;
    public Eye eye;
    public Inventory inventory;
    public LastPositionKeeper lastPositionKeeper;

    void Awake()
    {
        current = this;
        all.Add(this);
    }
}