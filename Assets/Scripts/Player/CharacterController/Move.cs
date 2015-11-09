using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    public static Move instance;

    public float speed = 6.0F;
    public float rotateSpeed = 6.0F;

    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Vector3 moveDirection = Vector3.zero;

    public float antigravitySpeed = 8;

    [HideInInspector]
    public float antigravityUntil = float.NegativeInfinity;
    public bool antigravity;

    public bool grounded;

    CharacterController controller;

    public void checkFlags()
    {
        antigravity = Time.time < antigravityUntil;
        grounded = controller.isGrounded;
    }

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0, Space.World);
        
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);
        move *= speed;
        moveDirection = new Vector3(move.x, moveDirection.y, move.z);
        
        if (grounded)
        {
            if (moveDirection.y < 0)
            {
                moveDirection.y = 0;
            }
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        //moveDirection.y -= gravity * Time.deltaTime;

        checkFlags();

        if (!antigravity)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0;
        }
        var totalMoveDirection = moveDirection;
        if (antigravity)
        {
            totalMoveDirection += Vector3.up * antigravitySpeed;
        }
        controller.Move(totalMoveDirection * Time.deltaTime);
    }
}