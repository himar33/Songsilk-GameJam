using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMove : MonoBehaviour
{
    
    public HUD hud;
    [Header("Variables player")]
    public float speed = 10f;
    public float runSpeed = 20f;
    public float jump = 15f;
    public float gravity = 5;

    public float ladderSpeed = 10f;

    private CharacterController controller;

    private Rigidbody physicsBody = null;

    Interactable interactableObject;
    public Animator animator;
    public AudioClip headShotClip;
    public Material skyboxMat;

    private Vector3 moveDirection;

    private bool canUpDown;

    [Header("Spawn Manager")]
    public Transform[] spawns;
    public Collider[] EndLevels;
    private Transform newPos;

    [Header("Animator")]
    [SerializeField]
    private AudioClip[] jumpClips;
    private AudioSource audioSource;

    public FieldOfView enemy;

    public enum State
    {
        MOVE, RUN, UP, TP, DEAD
    }
    public State state;

    private State lastState;
    // Start is called before the first frame update
    void Start()
    {
        newPos = spawns[0];
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        canUpDown = false;
        physicsBody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        handleInputs();
        
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Terrified"))
        {
            state = State.TP;
            animator.SetBool("isDead", false);
            enemy.gameObject.GetComponent<Animator>().speed = 1f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("HeadShot"))
        {
            state = State.TP;
            animator.SetBool("headShot", false);
        }
        

        switch (state)
        {
            case State.MOVE:

                Move();
                if (Input.GetButtonDown("Run"))
                {
                    state = State.RUN;
                    animator.SetBool("isRunning", true);
                }
                break;

            case State.RUN:

                MoveRun();
                if (Input.GetButtonUp("Run"))
                {
                    state = State.MOVE;
                    animator.SetBool("isRunning", false);
                }

                break;

            case State.UP:

                moveDirection = new Vector3(0, Input.GetAxis("Horizontal") * speed,0);
                controller.Move(moveDirection * Time.deltaTime);
                animator.SetFloat("SpeedY", Input.GetAxis("Horizontal"), 0.05f, Time.deltaTime);
                if (Input.GetAxis("Horizontal") < 0.1 && Input.GetAxis("Horizontal") > -0.1)
                    animator.speed = 0f;
                else
                    animator.speed = 1f;

                lastState = State.UP;
                break;

            case State.TP:

                gameObject.SetActive(false);
                gameObject.transform.position = newPos.position;
                gameObject.SetActive(true);
                break;

            case State.DEAD:
                break;

            default:
                break;
        }

        if (canUpDown){
            if (state == State.UP){
                if (Input.GetButtonDown("Interact"))
                {
                    state = State.MOVE;
                    animator.SetBool("IsClimbing", false);
                }
                return;
            }else{
                if (Input.GetButtonDown("Interact"))
                {
                    state = State.UP;
                    animator.SetBool("IsClimbing", true);
                    transform.Rotate(transform.forward);
                }
                return;
            }
        }

    }
    private void MoveRun()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * runSpeed, moveDirection.y, Input.GetAxis("Vertical") * runSpeed);
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
            if (i == 1)
            {
                RenderSettings.skybox = skyboxMat;
            }
        }

        if (other.CompareTag("Lanzadera"))
        {
            audioSource.PlayOneShot(headShotClip);
            state = State.DEAD;
            animator.SetBool("headShot", true);
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
        if (Input.GetButtonDown("Interact") && interactableObject != null)
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
