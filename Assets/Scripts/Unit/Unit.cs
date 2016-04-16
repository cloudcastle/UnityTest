using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public static List<Unit> all = new List<Unit>();

    public Activator activator;
    public Eye eye;
    public Inventory inventory;
    public LastPositionKeeper lastPositionKeeper;

    public UnitController controller;

    void Awake()
    {
        all.Add(this);
        if (controller == null) {
            controller = EmptyUnitController.instance;
        }
    }
}