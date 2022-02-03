using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMove : MonoBehaviour
{
    public HUD hud;
    [Header("Variables player")]
    public float speed = 10f;
    public float jump = 15f;
    public float gravity = 5;

    public float ladderSpeed = 10f;

    private CharacterController controller;

    private Rigidbody physicsBody = null;

    Interactable interactableObject;


    private Vector3 moveDirection;

    private bool upDown;
    private bool canUpDown;

    [Header("Spawn Manager")]
    public Transform[] spawns;
    public Collider[] EndLevels;
    private Transform newPos;

    public enum State
    {
        MOVE, UP, TP
    }
    private State state;
    // Start is called before the first frame update
    void Start()
    {
        upDown = false;
        canUpDown = false;
        physicsBody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }



    void Update()
    {
        handleInputs();


        switch (state)
        {
            case State.MOVE:

                Move();
                break;

            case State.UP:

                moveDirection = new Vector3(0, Input.GetAxis("Horizontal") * speed,0);
                controller.Move(moveDirection * Time.deltaTime);
                break;

            case State.TP:
                gameObject.SetActive(false);
                gameObject.transform.position = newPos.position;
                gameObject.SetActive(true);
                state = State.MOVE;
                break;

            default:
                break;
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
        }

        if (canUpDown){
            if (state == State.UP){
                if (Input.GetKeyDown(KeyCode.E)){
                    state = State.MOVE;
                }
                return;
            }else{
                if (Input.GetKeyDown(KeyCode.E)){
                    state = State.UP;
                }
                return;
            }
        }
    }

    private void Move()
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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            canUpDown = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactableObject = interactable;
            hud.OpenMessagePanel(other.transform.position);
        }

        int spawnIndex = 0;
        for (int i = 0; i < EndLevels.Length; i++)
        {
            spawnIndex++;
            if (other == EndLevels[i])
            {
                newPos = spawns[spawnIndex];
                state = State.TP;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            hud.CloseMessagePanel();
            interactableObject = null;
        }
        if (other.CompareTag("Ladder"))
        {
            state = State.MOVE;
            canUpDown = false;
        }
    }

    void handleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.OnInteract();
        }
    }
}
