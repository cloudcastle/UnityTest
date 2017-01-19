using UnityEngine;
using System.Collections;

public class ChangeGravityAbility : AbilityRB
{
    public float speed = 1;

    void Update() {
        var newDown = new Vector3(unit.controller.MoveGravity().x, -1, unit.controller.MoveGravity().y).normalized;
        transform.Rotate(new Vector3(unit.controller.MoveGravity().y, 0, unit.controller.MoveGravity().x).normalized * speed * Time.deltaTime);
        //rb.MoveRotation(rb.rotation * Quaternion.FromToRotation(transform.TransformDirection(Vector3.down), transform.TransformDirection(newDown)));
    }
}