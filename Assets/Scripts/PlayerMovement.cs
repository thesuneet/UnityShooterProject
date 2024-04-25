using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    private CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;
    
    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Resetting the default velocity
        if(isGrounded && velocity.y <0)
        {
            velocity.y = -2f; 
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create move vector
        Vector3 move = transform.right * x + transform.forward * z; //right = red axis; forward = blue axis

        controller.Move(move * speed * Time.deltaTime);

        // Check if player can jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            // Going up
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Falling down
        velocity.y += gravity * Time.deltaTime;

        // Executing the jump
        controller.Move(velocity * Time.deltaTime);

        if(lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            // For later use

        }
        else
        {
            isMoving= false;
            // Also for later use
        }

        lastPosition = gameObject.transform.position;
    }
}
