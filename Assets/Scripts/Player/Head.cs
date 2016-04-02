using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    public float verticalRotateSpeed = 6.0F;

    float angle;

    void Update()
    {
        if (TimeManager.paused)
        {
            return;
        }
        angle -= Input.GetAxis("Mouse Y") * GameManager.game.settings.mouseSpeed * verticalRotateSpeed;
        angle = Mathf.Clamp(angle, -90, 90);
        transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}