using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class RelativeMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    public float jumpSpeed = 15f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10f;
    public float rotSpeed = 15f;
    public float moveSpeed = 6f;
    public float minFall = -1.5f;
    private float vertSpeed;
    private CharacterController charController;
    private ControllerColliderHit contact;

    void Start()
    {
        vertSpeed = minFall;
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool hitGround = false;
        RaycastHit hit;
        Vector3 movement = Vector3.zero;
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (vertSpeed < 0 && Physics.Raycast(origin, Vector3.down, out hit))
        {
            float check = (charController.height + charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = target.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);

            movement = (right * horInput) + (forward * vertInput);
            movement *= moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity) vertSpeed = terminalVelocity;
        }
        //*
        if (charController.isGrounded)
        {
            if (Vector3.Dot(movement, contact.normal) < 0)
            {
                movement = contact.normal * moveSpeed;
            }
        }
        //*

        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        charController.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
    }
}