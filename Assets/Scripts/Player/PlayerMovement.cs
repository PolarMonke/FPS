using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Transform Camera;


    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;

    private bool isGrounded;
    //private bool isMoving;

    private Vector3 lastPosition = new Vector3(0f,0f,0f);
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        CheckGround();
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Camera.transform.right * x + Camera.transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);


        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //if (lastPosition != gameObject.transform.position && isGrounded == true)
        //{
            //isMoving = true;
        //}
        //else 
        //{
            //isMoving = false;
        //}
        lastPosition = gameObject.transform.position;
    }
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    public void MultiplySpeed(int multiplier)
    {
        speed *= multiplier;
    }
    public void DivideSpeed(int divisor)
    {
        speed /= divisor;
    }
    
}
