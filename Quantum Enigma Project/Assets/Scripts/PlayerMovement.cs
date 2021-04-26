using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public AudioSource noteSound;
    public AudioClip[] noteSoundArray;
    public AudioSource[] movementSoundArray;

    public float speed = 12f;
    public float sprint = 18f;
    public float gravity = -9f;
    public float jump = 3f;
    public float groundDistance = 0.4f;

    Vector3 velocity;
    AudioSource moveSound;
    AudioSource jumpSound;
    bool isGrounded;
    bool isMoving;
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
            moveSound = movementSoundArray[1];
            Debug.Log("Sprint");
        }
        else
        {
            moveSound = movementSoundArray[0];
            final_speed = speed;
        }

        if (x == 0  && z == 0)
		{
            isMoving = false;
		}
        else
		{
            isMoving = true;
		}

        if (isMoving)
		{ 
            if (!moveSound.isPlaying)
			{
                moveSound.volume = Random.Range(0.3f, 0.4f);
                moveSound.pitch = Random.Range(0.8f, 1.2f);
                moveSound.Play();
            } 
		}
        else
		{
            moveSound.Stop();
		}

         Vector3 move = transform.right * x + transform.forward * z;
         controller.Move(move * final_speed * Time.deltaTime);



        jumpSound = movementSoundArray[2];
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            jumpSound.volume = Random.Range(0.15f, 0.3f);
            jumpSound.pitch = Random.Range(0.8f, 1.1f);
            jumpSound.Play();
            Debug.Log("Jump");
        }
        
        if(!isGrounded)
		{
            moveSound.Stop();
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        noteSound.clip = noteSoundArray[Random.Range(0, noteSoundArray.Length)];

        if (other.CompareTag("Note"))
        {
            noteSound.volume = Random.Range(0.1f, 0.2f);
            noteSound.pitch = Random.Range(0.8f, 1.1f);
            noteSound.Play();
        }
        else
		{
            noteSound.Stop();
		}
    }

}
