using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : UnitController
{
    public static Player instance;

    public Transform inventoryAreas;

    public MainCamera mainCamera;

    public Unit current;

    public Cooldown possessCooldown;

    void Awake() {
        instance = this;
        mainCamera = FindObjectOfType<MainCamera>();
        onGainControl += OnGainControl;
    }

    void Start() {
         possessCooldown = new Cooldown(0.25f);
         var startUnit = FindObjectOfType<StartUnit>();
         if (startUnit != null) {
             Debug.Log("startUnit.unit = " + startUnit.unit);
             Possess(startUnit.unit, animate: false);
         } else {
             Debug.Log("Possess Some Unit");
             Possess(FindObjectOfType<Unit>(), animate: false);
         }
         new ValueTracker<Unit>(v => current = v, () => current);
    }

    void OnGainControl(Unit unit) {
        current = unit;
    }

    public void Possess(Unit unit, bool animate = true) {
        if (possessCooldown.OnCooldown()) {
            return;
        }
        possessCooldown.StartCooldown();
        if (current != null) {
            LoseControl(current);
        }
        GainControl(unit);
        if (animate) {
            mainCamera.MoveTo(unit.cameraPlace.transform);
        } else {
            mainCamera.MoveToInstant(unit.cameraPlace.transform);
        }
    }

    public override bool Activate() {
        return Input.GetButtonDown("Activate");
    }

    public override Vector2 Mouse() {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public override bool Jump() {
        return Input.GetButtonDown("Jump");
    }

    public override float Scale() {
        return Input.GetAxis("Scale");
    }

    public override bool Crouch() {
        return Input.GetButton("Crouch");
    }

    public override bool Jetpack() {
        return Input.GetButton("Jump");
    }

    public override Vector2 Move() {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public override bool NextItem() {
        return Input.GetButtonDown("Next Item") || Input.GetAxisRaw("Mouse ScrollWheel") < 0;
    }

    public override bool PrepareThrow() {
        return Input.GetButtonDown("Throw");
    }

    public override bool PreviousItem() {
        return Input.GetButtonDown("Previous Item") || Input.GetAxisRaw("Mouse ScrollWheel") > 0;
    }

    public override bool Slowmo() {
        return Input.GetButton("Slowmo");
    }

    public override bool Rewind() {
        return Input.GetButton("Rewind");
    }

    public override bool Run() {
        return Input.GetButton("Run");
    }

    public override bool Throw() {
        return Input.GetButtonUp("Throw");
    }

    public override bool ToggleTimestop() {
        return Input.GetButtonDown("Timestop");
    }

    public override bool Undo() {
        return Input.GetButton("Undo");
    }
}