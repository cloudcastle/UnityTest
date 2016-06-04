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
    public Head head;
    public CameraPlace cameraPlace;
    public Undo undo;
    public CharacterController characterController;
    public Rewind rewind;
    public Slowmo slowmo;
    public Gravity gravity;

    public UnitController controller;

    void Awake()
    {
        activator = GetComponentInChildren<Activator>();
        eye = GetComponentInChildren<Eye>();
        inventory = GetComponentInChildren<Inventory>();
        lastPositionKeeper = GetComponentInChildren<LastPositionKeeper>();
        head = GetComponentInChildren<Head>();
        cameraPlace = GetComponentInChildren<CameraPlace>();
        undo = GetComponentInChildren<Undo>();
        characterController = GetComponentInChildren<CharacterController>();
        rewind = GetComponentInChildren<Rewind>();
        slowmo = GetComponentInChildren<Slowmo>();
        gravity = GetComponentInChildren<Gravity>();

        all.Add(this);
    }

    void Start() {
        if (controller == null) {
            controller = EmptyUnitController.instance;
        }
        new ValueTracker<UnitController>(v => controller = v, () => controller);
    }
}