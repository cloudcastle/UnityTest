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

    Move move;
    float baseSpeed;
    float baseJumpSpeed;
    float baseGravity;

    Crouch crouch;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        baseStepOffset = characterController.stepOffset;
        baseRadius = characterController.radius;
        baseHeight = characterController.height;

        move = GetComponent<Move>();
        baseSpeed = move.speed;
        baseJumpSpeed = move.jumpSpeed;
        baseGravity = move.gravity;

        crouch = GetComponent<Crouch>();
    }

    float CrouchHeightMultiplier()
    {
        return crouch == null ? 1 : crouch.currentHeightMultiplier;
    }

    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        currentScale *= Mathf.Pow(2, Input.GetAxis("Scale") * Time.deltaTime * scaleSpeed);
        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

        scalableBody.localScale = Vector3.one * currentScale;
        
        characterController.stepOffset = baseStepOffset * currentScale;
        characterController.height = baseHeight * currentScale * CrouchHeightMultiplier();
        characterController.radius = baseRadius * currentScale;

        move.speed = baseSpeed * currentScale;
        move.jumpSpeed = baseJumpSpeed * currentScale;
        move.gravity = baseGravity * currentScale;
    }
}