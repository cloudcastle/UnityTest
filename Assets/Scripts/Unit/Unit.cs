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
        activator = GetComponentInChildren<Activator>();
        eye = GetComponentInChildren<Eye>();
        inventory = GetComponentInChildren<Inventory>();
        lastPositionKeeper = GetComponentInChildren<LastPositionKeeper>();

        all.Add(this);
        if (controller == null) {
            controller = EmptyUnitController.instance;
        }
    }
}