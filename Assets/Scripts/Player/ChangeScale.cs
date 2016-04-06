using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Move))]
public class ChangeScale : MonoBehaviour
{
    public float scaleSpeed = 1;
    public Transform scalableBody;
    public float currentScale = 1;
    public float minScale = 0.15F;
    public float maxScale = float.PositiveInfinity;

    CharacterController characterController;
    float baseStepOffset;
    float baseHeight;
    float baseRadius;
    float baseSkinWidth;
    float baseMinMoveDistance;

    Crouch crouch;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        baseStepOffset = characterController.stepOffset;
        baseRadius = characterController.radius;
        baseHeight = characterController.height;
        baseSkinWidth = characterController.skinWidth;

        crouch = GetComponent<Crouch>();
    }

    float CrouchHeightMultiplier()
    {
        return crouch == null ? 1 : crouch.currentHeightMultiplier;
    }

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        currentScale *= Mathf.Pow(2, Input.GetAxis("Scale") * Time.deltaTime * scaleSpeed);
        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

        scalableBody.localScale = Vector3.one * currentScale;
        
        characterController.stepOffset = baseStepOffset * currentScale;
        characterController.height = baseHeight * currentScale * CrouchHeightMultiplier();
        characterController.radius = baseRadius * currentScale;
        characterController.skinWidth = baseSkinWidth * currentScale;
    }
}