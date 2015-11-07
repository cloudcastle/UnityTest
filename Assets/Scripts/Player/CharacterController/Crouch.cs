using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Crouch : MonoBehaviour
{
    public float crouchSpeed = 8.0F;
    public float uncrouchSpeed = 8.0F;
    public float maxHeight = 2;
    public float minHeight = 0;
    float currentHeight = 2;

    private Vector3 moveDirection = Vector3.zero;

    CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        if (Input.GetButton("Crouch"))
        {
            currentHeight = Mathf.Max(currentHeight - crouchSpeed * Time.deltaTime, minHeight);
        }
        else
        {
            currentHeight = Mathf.Min(currentHeight + uncrouchSpeed * Time.deltaTime, maxHeight);
        }
        characterController.height = currentHeight;
    }
}