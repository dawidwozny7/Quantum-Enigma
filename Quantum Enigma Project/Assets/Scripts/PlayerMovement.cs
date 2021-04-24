using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 12f;
    public float sprint = 18f;
    public float gravity = -9f;
    public float jump = 3f;

    Vector3 velocity;
    bool isGrounded;
    float final_speed;


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Crouch
          // Problem: when about to crouch the height goes down smoothly, but not the other way around
        if (Input.GetKey(KeyCode.C))
        {
            controller.height = 2f;
        }
        else
        {
            controller.height = 3f;
        } 

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            final_speed = sprint;
            Debug.Log("Sprint");
        }
        else
        {
            final_speed = speed;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * final_speed * Time.deltaTime);

        // Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            Debug.Log("Jump");
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
