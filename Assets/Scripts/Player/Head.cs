using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    public float verticalRotateSpeed = 6.0F;

    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        transform.RotateAround(transform.position, transform.right, -Input.GetAxis("Mouse Y") * verticalRotateSpeed);
    }
}