using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Crouch : MonoBehaviour
{
    public float crouchSpeed = 4.0F;
    public float uncrouchSpeed = 4.0F;
    public float maxHeightMultiplier = 1;
    public float minHeightMultiplier = 0.25F;
    public float currentHeightMultiplier = 1;

    CharacterController characterController;
    float baseHeight;

    ChangeScale changeScale;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        baseHeight = characterController.height;

        changeScale = GetComponent<ChangeScale>();
    }

    void Update()
    {
        if (TimeManager.paused)
        {
            return;
        }
        if (Input.GetButton("Crouch"))
        {
            currentHeightMultiplier = Mathf.Max(currentHeightMultiplier - crouchSpeed * Time.deltaTime, minHeightMultiplier);
        }
        else
        {
            currentHeightMultiplier = Mathf.Min(currentHeightMultiplier + uncrouchSpeed * Time.deltaTime, maxHeightMultiplier);
        }
        if (changeScale == null)
        {
            characterController.height = baseHeight * currentHeightMultiplier;
        }
    }
}