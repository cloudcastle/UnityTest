using UnityEngine;
using System.Collections;

public class Head : Ability
{
    public float verticalRotateSpeed = 6.0F;

    void Update()
    {
        if (TimeManager.Paused || TimeManager.instance.Undoing())
        {
            return;
        }
        var angle = transform.localEulerAngles.x;
        angle -= Controller.Mouse().y * GameManager.game.settings.mouseSpeed * verticalRotateSpeed;
        angle = Extensions.NormalizeAngle(angle);
        angle = Mathf.Clamp(angle, -90, 90);
        transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}