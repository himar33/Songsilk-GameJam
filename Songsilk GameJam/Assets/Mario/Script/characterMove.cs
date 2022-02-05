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

    private bool canUpDown;

    [Header("Spawn Manager")]
    public Transform[] spawns;
    public Collider[] EndLevels;
    private Transform newPos;

    Animator animator;
    [Header("Animator")]
    [SerializeField]
    private AudioClip[] jumpClips;
    private AudioSource audioSource;

    public enum State
    {
        MOVE, UP, TP
    }
    public State state;
       
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
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
                animator.SetFloat("SpeedY", Input.GetAxis("Horizontal"), 0.05f, Time.deltaTime);
                if (Input.GetAxis("Horizontal") < 0.1 && Input.GetAxis("Horizontal") > -0.1)
                {
                    animator.speed = 0f;
                }
                else
                {
                    animator.speed = 1f;
                }
                break;

            case State.TP:
                gameObject.SetActive(false);
                gameObject.transform.position = newPos.position;
                gameObject.SetActive(true);
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
                    animator.SetBool("IsClimbing", false);
                }
                return;
            }else{
                if (Input.GetKeyDown(KeyCode.E)){
                    state = State.UP;
                    animator.SetBool("IsClimbing", true);
                    transform.Rotate(transform.forward);
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
            animator.SetBool("Jump", false);
            moveDirection.y = -1; //Ensures contact with the ground
            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                //JumpSound();
                moveDirection.y = jump;
            }
        }
        else
        {
            // Apply Gravity
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravity * Time.deltaTime);
            animator.SetBool("Jump", true);
        }
        
        controller.Move(moveDirection * Time.deltaTime);

        //Set controller rotation
        Vector3 movement = new Vector3(moveDirection.x, 0.0f, moveDirection.z);
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        }

        //Set Animator values
        float velocity = Vector3.Dot(movement.normalized, transform.forward);
        animator.SetFloat("Speed", velocity, 0.1f, Time.deltaTime);
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
            animator.SetBool("IsClimbing", false);
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

    private void JumpSound()
    {
        AudioClip clip = GetRandomClip();
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(clip);
        audioSource.volume = 1.0f;
    }

    private AudioClip GetRandomClip()
    {
        return jumpClips[UnityEngine.Random.Range(0, jumpClips.Length)];
    }
   
    public Transform GetSpawnPos()
    {
        return newPos;
    }
}
