using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMove : MonoBehaviour
{
    
    public float speed = 10f;
    public float jump = 15f;

    private CharacterController controller;

    private Rigidbody physicsBody = null;
    

    private Vector3 moveDirection;
    public float gravity = 5;

    // Start is called before the first frame update
    void Start()
    {
        physicsBody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }
    /*
    private void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);

        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravity* Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }
    */
     void Update()
{

        moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, moveDirection.y, Input.GetAxis("Vertical") * speed);
        // Apply direction to controller

        if (controller.isGrounded)
        {
            moveDirection.y = -1; //Ensures contact with the ground

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }

        }
        else
        {
            // Apply Gravity
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravity * Time.deltaTime);
        }

    controller.Move(moveDirection * Time.deltaTime);

    //Debug.Log(moveDirection.y);
}
}
