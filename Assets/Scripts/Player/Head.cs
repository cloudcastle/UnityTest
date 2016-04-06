using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    public float verticalRotateSpeed = 6.0F;

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        var angle = transform.localEulerAngles.x;
        angle -= Input.GetAxis("Mouse Y") * GameManager.game.settings.mouseSpeed * verticalRotateSpeed;
        angle = Extensions.NormalizeAngle(angle);
        angle = Mathf.Clamp(angle, -90, 90);
        transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}